using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA.PathFinding
{
    //El nodo tiene que ser como una struct, solo tener los datos necesarios para ser tan liviano como sea posible.
    public class Nodo
    {
        public List<Nodo> neighbours = new List<Nodo>();
        public float instanceHashCode;
        public Vector3 position;
    }
}


