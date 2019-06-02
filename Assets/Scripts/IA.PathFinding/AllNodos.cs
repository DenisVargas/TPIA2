using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;

/*
    El array de nodos podría estar en cualquier lugar, no haría falta una clase para guardarlos ya que los podemos asignar por inspector.
    La función Near Nodo tranquilamente podria estar dentro de la clase misma que requiere su nodo mas cercano.
 */


public class AllNodos : MonoBehaviour 
{
    public static Nodo[] allnodos;


    public void Awake()
    {
        allnodos = FindObjectsOfType<Nodo>();
    }

    //Devuelve el nodo más cercano a mi posición.
    public static Nodo NearNodo(Vector3 posicion)
    {
        float minDistance = 999999;
        Nodo nearnodo = null;

        for (int i = 0; i < allnodos.Length; i++)
        {
           var currentDistance = Vector3.Distance(allnodos[i].transform.position,posicion);
            if (currentDistance<minDistance)
            {
                minDistance = currentDistance;
                nearnodo = allnodos[i];
            }
           
        }
        return nearnodo;
    }
}
