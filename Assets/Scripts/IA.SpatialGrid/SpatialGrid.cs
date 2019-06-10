using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IA.SpatialGrid
{
    public class SpatialGrid : MonoBehaviour
    {
        #region Variables
        public Vector2 origin;
        public Vector2 cellSize;
        public Vector2 gridSize = new Vector2(1,1);

        public Vector2 ActiveCell
        {
            get { return _active; }

            set
            {
                if (value.x >= 0 && value.x < gridSize.x && value.y >= 0 && value.y < gridSize.y)
                    _active = value;
            }
        }

        public HashSet<IGridEntity>[,] _buckets;
        public Dictionary<IGridEntity, Vector2> _lastPositions;

        public readonly Vector2 outside = new Vector2(-1, -1);
        public readonly IGridEntity[] EmptyGridEntityCollection = new IGridEntity[0];

        Vector2 _active;
        #endregion

        private void Awake()
        {
            _lastPositions = new Dictionary<IGridEntity, Vector2>();
            _buckets = new HashSet<IGridEntity>[(int)gridSize.x, (int)gridSize.y];
            _active = new Vector2(0,0);

            for (int i = 0; i < gridSize.x; i++)
                for (int j = 0; j < gridSize.y; j++)
                    _buckets[i, j] = new HashSet<IGridEntity>();

            IEnumerable<IGridEntity> entities = LazyGetChildren(transform)
                .Select(x => x.GetComponent<IGridEntity>())
                .Where(x => x != null);

            foreach (var entity in entities)
            {
                entity.OnMoveEvent += UpdateEntityPosition;
                UpdateEntityPosition(entity);
            }
        }

        /// <summary>
        /// Filtra y retorna todas las entidades que se encuentren en la grilla teniendo en cuenta su posición dentro de la misma.
        /// </summary>
        /// <param name="From">Origen menor en el mundo</param>
        /// <param name="To">Origen mayor en el mundo</param>
        /// <param name="filterByPosition">Criterio secundario. Permite agregar un paso extra al filtrado.</param>
        /// <returns> Lista de entidades en el rango indicado, dentro de la grilla.</returns>
        public IEnumerable<IGridEntity> GetEntitiesInRange(Vector3 From, Vector3 To, Func<Vector3, bool> filterByPosition)
        {
            Vector3 _from = new Vector3(Mathf.Min(From.x, To.x), 0, Mathf.Min(From.z, To.z));//Posición del Ancla A en el mundo.
            Vector3 _to = new Vector3(Mathf.Max(From.x, To.x), 0, Mathf.Max(From.z, To.z));  //Posición del Ancla B en el mundo.

            Vector2 fromCoord = GetPositionInGrid(_from);                                    //Posición del Ancla A en la grilla.
            Vector2 toCoord = GetPositionInGrid(_to);                                        //Posición del Ancla B en la grilla.

            if (!IsInsideGrid(fromCoord) && !IsInsideGrid(toCoord))
                return EmptyGridEntityCollection;

            //¡Ojo que clampea a 0,0 el Outside! TODO: Checkear cuando descartar el query si estan del mismo lado
            fromCoord = new Vector2(Mathf.Clamp(fromCoord.x, origin.x, gridSize.x), Mathf.Clamp(fromCoord.y, origin.y, gridSize.y));//Fix Área Ancla A.
            toCoord = new Vector2(Mathf.Clamp(toCoord.x, origin.x, gridSize.x), Mathf.Clamp(toCoord.y, origin.y, gridSize.y));      //Fix Área Ancla B.

            //TODO p/Alumno: Cambiar por Where/Take --> FUck ThIs ShIt

            // Creamos tuplas de cada celda, no es necesario el takeWhile porque las posiciones están clampeadas.
            IEnumerable<float> cols = GenerateLazyCollection(fromCoord.x, x => x + 1);
            //.TakeWhile(colIndex => colIndex <= gridSize.x && colIndex <= toCoord.x);//Toma los elementos mientras estos estén dentro de los límites de la grilla.

            IEnumerable<float> rows = GenerateLazyCollection(fromCoord.y, y => y + 1);
            //.TakeWhile(rowIndex => rowIndex <= gridSize.y && rowIndex <= toCoord.y);//Toma los elementos mientras estos estén dentro de los límites de la grilla.

            //Colección de Coordenadas de celdas. Combina las 2 coordenadas generadas en un vector.
            IEnumerable<Vector2> cells = cols.SelectMany(
                col => rows.Select(
                    row => new Vector2(col, row)
                )
            );

            // Iteramos las que queden dentro del criterio.
            return cells.SelectMany(cellpos => _buckets[(int)cellpos.x, (int)cellpos.y])
                        .Where(entity => _from.x <= entity.positionInWorld.x && entity.positionInWorld.x <= _to.x
                                      && _from.z <= entity.positionInWorld.z && entity.positionInWorld.z <= _to.z)
                        .Where(x => filterByPosition(x.positionInWorld));
        }
        /// <summary>
        /// Actualiza la posicion de una entidad dentro de la grilla.
        /// </summary>
        /// <param name="entity">La entidad que va a calcular su posición en la grilla</param>
        public void UpdateEntityPosition(IGridEntity entity)
        {

            MonoBehaviour.print(entity + " its moving...");

            //Chequeamos la posicion actual de la entidad y si hay un registro anterior de el, de lo contrario su registro sera "afuera"
            Vector2 lastPos = _lastPositions.ContainsKey(entity) ? _lastPositions[entity] : outside;
            Vector2 currentPos = GetPositionInGrid(entity.positionInWorld);

            //Misma posición, no necesito hacer nada
            if (lastPos == currentPos)
            {
                MonoBehaviour.print(entity + " Position its the same, no action required.");
                return;
            }

            //Si la nueva posición cambio y esta dentro de la grilla...
            bool toChange = IsInsideGrid(currentPos);
            if (toChange)
            {
                //Lo "sacamos" de la celda anterior si la posición estaba dentro de la grilla.
                if (lastPos != outside)
                    _buckets[(int)lastPos.x, (int)lastPos.y].Remove(entity);
                //Lo "metemos" a la celda nueva, o lo sacamos si salio de la grilla
                _buckets[(int)currentPos.x, (int)currentPos.y].Add(entity);
                _lastPositions[entity] = currentPos;

                MonoBehaviour.print(entity + " Position has been actualized, last position was: " + lastPos + " and his new position its: " + currentPos);
            }
            else //Esta afuera de la grilla
            {
                MonoBehaviour.print(entity + " Its outside of the Grid. ");
                _lastPositions.Remove(entity);
            }
        }
        /// <summary>
        /// Dada una posicion en el mundo, devuelve la coordenada correspondiente a la grilla.
        /// </summary>
        /// <param name="pos">Posción en el mundo</param>
        /// <returns>Posición en la grilla</returns>
        public Vector2 GetPositionInGrid(Vector3 pos)
        {
            //quita la diferencia, divide segun las celdas y floorea
            //Devuelve el índice de la celda en el que está ubicado la posición.
            return new Vector2(Mathf.Abs(Mathf.FloorToInt((pos.x - origin.x) / cellSize.x)),
                                Mathf.Abs(Mathf.FloorToInt((pos.z - origin.y) / cellSize.y)));
        }
        /// <summary>
        /// Determina si una posición se encuentra dentro de los límites de la grilla.
        /// </summary>
        /// <param name="position">Posición a comprobar</param>
        /// <returns>Verdadero si la posicion se encuentra dentro de la grilla</returns>
        public bool IsInsideGrid(Vector2 position)
        {
            return origin.x <= position.x && position.x < gridSize.x
                && origin.y <= position.y && position.y < gridSize.y;
        }

        private void OnDestroy()
        {
            //Esto es para des-suscribir los agentes al evento.
            var entities = LazyGetChildren(transform)
                .Select(x => x.GetComponent<IGridEntity>())
                .Where(x => x != null);

            foreach (var entity in entities)
                entity.OnMoveEvent -= UpdateEntityPosition;
        }

        private static IEnumerable<Transform> LazyGetChildren(Transform parent)
        {
            foreach (Transform child in parent)
                yield return child;
        }
        IEnumerable<T> GenerateLazyCollection<T>(T seed, Func<T, T> mutate)
        {
            T accum = seed;
            while (true)
            {
                yield return accum;
                accum = mutate(accum);
            }
        }

        #region GRAPHIC REPRESENTATION
        public bool AreGizmosShutDown;
        public bool activatedGrid;
        public bool showLogs = true;
        private void OnDrawGizmos()
        {
            var rows = GenerateLazyCollection(origin.y, curr => curr + cellSize.y)
                    .Select(row => Tuple.Create(new Vector3(origin.x, 0, row),
                                                new Vector3(origin.x + cellSize.x * gridSize.x, 0, row)));

            //equivalente de rows
            /*for (int i = 0; i <= height; i++)
            {
                Gizmos.DrawLine(new Vector3(x, 0, z + cellHeight * i), new Vector3(x + cellWidth * width,0, z + cellHeight * i));
            }*/

            var cols = GenerateLazyCollection(origin.x, curr => curr + cellSize.x)
                       .Select(col => Tuple.Create(new Vector3(col, 0, origin.y), new Vector3(col, 0, origin.y + cellSize.y * gridSize.y)));

            var allLines = rows.Take((int)gridSize.x + 1).Concat(cols.Take((int)gridSize.y + 1));

            foreach (var elem in allLines)
            {
                Gizmos.DrawLine(elem.Item1, elem.Item2);
            }

            if (_buckets == null || AreGizmosShutDown) return;

            var originalCol = GUI.color;
            GUI.color = Color.red;
            if (!activatedGrid)
            {
                IEnumerable<IGridEntity> allElems = Enumerable.Empty<IGridEntity>();
                foreach (var elem in _buckets)
                    allElems = allElems.Concat(elem);

                int connections = 0;
                foreach (var ent in allElems)
                {
                    foreach (var neighbour in allElems.Where(x => x != ent))
                    {
                        Gizmos.DrawLine(ent.positionInWorld, neighbour.positionInWorld);
                        connections++;
                    }
                    if (showLogs)
                        Debug.Log("tengo " + connections + " conexiones por individuo");
                    connections = 0;
                }
            }
            else
            {
                int connections = 0;
                foreach (var elem in _buckets)
                {
                    foreach (var ent in elem)
                    {
                        foreach (var n in elem.Where(x => x != ent))
                        {
                            Gizmos.DrawLine(ent.positionInWorld, n.positionInWorld);
                            connections++;
                        }
                        if (showLogs)
                            Debug.Log("tengo " + connections + " conexiones por individuo");
                        connections = 0;
                    }
                }
            }

            GUI.color = originalCol;
            showLogs = false;
        }
        #endregion
    }
}
