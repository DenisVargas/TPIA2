using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA.SpatialGrid;

namespace IA.SpatialGrid.Selections
{
    public class BoxSelection
    {
        public readonly SpatialGrid targetGrid;
        public readonly float width = 15f;
        public readonly float height = 30f;

        public BoxSelection(SpatialGrid targetGrid, float width, float height)
        {
            this.targetGrid = targetGrid;
            this.width = width;
            this.height = height;
        }

        public IEnumerable<IGridEntity> Select(Vector3 From)
        {
            var h = height * 0.5f;
            var w = width * 0.5f;

            return targetGrid.GetEntitiesInRange(
                From + new Vector3(-w, 0, -h),
                From + new Vector3(w, 0, h),
                x => true);
        }
    }

    public class CircleSelection
    {
        public readonly SpatialGrid targetGrid;
        public readonly Vector3 originWorldPosition;
        public float radius = 20f;

        public CircleSelection(SpatialGrid targetGrid, float radius)
        {
            this.targetGrid = targetGrid;
            this.radius = radius;
        }

        public IEnumerable<IGridEntity> SelectQuery(Vector3 From)
        {
            //Si es una esfera
            //creo una "caja" con las dimensiones deseadas, y luego filtro segun distancia para formar el círculo
            return targetGrid.GetEntitiesInRange(
                From + new Vector3(-radius, 0, -radius),
                From + new Vector3(radius, 0, radius),
                entityPos => {
                    var position2d = entityPos - From;
                    position2d.y = 0;
                    return position2d.sqrMagnitude < radius * radius; //Una forma mas "performante" de comparar distancias
                });
        }
    }
}
// void OnDrawGizmos() 
//     {
//         if (targetGrid == null)
//             return;

//         //Flatten the sphere we're going to draw
//         Gizmos.color = Color.cyan;
//         if (isBox)
//             Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
//         else
//         {
//             Gizmos.matrix *= Matrix4x4.Scale(Vector3.forward + Vector3.right);
//             Gizmos.DrawWireSphere(transform.position, radius);
//         }

//         if (Application.isPlaying)
//         {
//             selected = Query();
//         }
//     }

//     private void OnGUI()
//     {
//         GUI.Label( new Rect(0,0,20,20), "HOLA");
//     }
