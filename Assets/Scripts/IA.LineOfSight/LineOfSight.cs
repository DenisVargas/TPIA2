using UnityEngine;

namespace IA.LineOfSight
{
    public static class LineOfSightEntity
    {
        /// <summary>
        /// Crea una instancia nueva de LineOfSight basándose en cualquier UnityEngine.Transform
        /// </summary>
        /// <param name="range">Rango máximo de la visión</param>
        /// <param name="angle">Ángulo máximo de la visión</param>
        public static LineOfSight CreateSightEntity(this Transform _origin, float range, float angle)
        {
            return new LineOfSight(_origin, range, angle);
        }
    }

    public class LineOfSight
    {
        public LayerMask visibles { get; set; }

        Transform _target;
        Transform _origin;
        float _range;
        float _angle;

        /// <summary>
        /// Crea un nueva Línea de visión.
        /// </summary>
        /// <param name="origin">El origen de coordenadas para el cálculo de visión</param>
        /// <param name="range">La distancia máxima de la visión</param>
        /// <param name="angle">El ángulo máximo de visión</param>
        public LineOfSight(Transform origin, float range, float angle)
        {
            _origin = origin;
            _range = range;
            _angle = angle;
            LayerMask visibility = ~0;
        }
        /// <summary>
        /// Crea un nueva Línea de visión.
        /// </summary>
        /// <param name="origin">El origen de coordenadas para el cálculo de visión</param>
        /// <param name="target">Objetivo a comprobar</param>
        /// <param name="range">La distancia máxima de la visión</param>
        /// <param name="angle">El ángulo máximo de visión</param>
        /// <param name="v">Elementos visibles para esta entidad</param>
        public LineOfSight(Transform origin, Transform target, float range, float angle, LayerMask v)
        {
            _target = target;
            _origin = origin;
            _range = range;
            _angle = angle;
            visibles = v;
        }

        /// <summary>
        /// Guarda una referencia a un objetivo recurrente.
        /// Habilita el uso de .IsInSight sin necesidad de especificar un objetivo.
        /// </summary>
        /// <param name="target">Objetivo recurrente</param>
        public LineOfSight setUniqueTarget(Transform target)
        {
            _target = target;
            return this;
        }

        /// <summary>
        /// Indica si un objetivo está dentro de la línea de visión
        /// </summary>
        /// <returns> verdadero si el objetivo recurrente está dentro de la línea de visión</returns>
        public bool IsInSight()
        {
            Vector3 positionDiference = _target.position - _origin.transform.position;
            float distance = positionDiference.magnitude;
            float angleToTarget = Vector3.Angle(_origin.transform.forward, positionDiference);

            positionDiference.Normalize();

            if (distance > _range || angleToTarget > _angle) return false;

            RaycastHit hitInfo;
            if (Physics.Raycast(_origin.position, positionDiference, out hitInfo, _range, visibles))
                return hitInfo.transform == _target;

            return true;
        }
        /// <summary>
        /// Indica si el objetivo específicado está dentro de la línea de visión
        /// </summary>
        /// <param name="target">Objetivo a comprobar</param>
        /// <returns>Verdadero si el Objetivo específicado está dentro de la línea de visión</returns>
        public bool IsInSight(Transform target)
        {
            Vector3 positionDiference = target.position - _origin.transform.position;
            float distance = positionDiference.magnitude;
            float angleToTarget = Vector3.Angle(_origin.transform.forward, positionDiference);

            positionDiference.Normalize();

            if (distance > _range || angleToTarget > _angle) return false;

            RaycastHit hitInfo;
            if (Physics.Raycast(_origin.position, positionDiference, out hitInfo, _range, visibles))
                return hitInfo.transform == target;

            return true;
        }
    }
}
