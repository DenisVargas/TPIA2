﻿using System.Collections;
using System.Collections.Generic;

namespace IA.PathFinding
{
    //El nodo tiene que ser como una struct, solo tener los datos necesarios para ser tan liviano como sea posible.
    public class Nodo
    {
        public List<Nodo> neighbours = new List<Nodo>();
    }
}


