using System;
using System.Collections.Generic;
using UnityEngine;
using IA.PathFinding;
using IA.PathFinding.Methods;

//La utilidad de Finder es Setear a un Walker su camino
public class Finder : MonoBehaviour
{
    public Nodo start;
    public Nodo End;
    public WalkerSimple walker;
    public enum FindingMethod
    {
        BFS,
        DFS,
        Dijkstra,
        AStar,
        ThetaStar
    }
    public FindingMethod method;

    //FieldOfView
    //Nota: Esto es usado en FinderThetaStar
    public float VisibilityRange;
    public LayerMask visibles = ~0;

    private void Start()
    {
        List<Nodo> path = new List<Nodo>();

        //Aca va a cambiar el método.
        switch (method)
        {
            case FindingMethod.BFS:
                path = BFS.Run(start, satisfies, Expand);
                break;
            case FindingMethod.DFS:
                path = DFS.Run(start, satisfies, Expand);
                break;
            case FindingMethod.Dijkstra:
                path = Dijkstra.Run(start, satisfies, ExpandWeighted);
                break;
            case FindingMethod.AStar:
                path = AStar.Run(start, satisfies, ExpandWeighted, heuristic);
                break;
            case FindingMethod.ThetaStar:
                path = ThetaStar.Run(start, satisfies, ExpandWeighted, heuristic, insigth, cost);
                break;
            default:
                break;
        }

        // foreach (var nodo in path )
        // {
        //     Debug.Log(nodo.name);
        // }

        walker.waypoints = path;
    }

    bool satisfies(Nodo n)
    {
        return n == End;
    }

    public List<Nodo> Expand(Nodo n)
    {
        return n.neighbours;
    }

    //Expand    
    public List<Tuple<Nodo, float>> ExpandWeighted(Nodo n)//le pasamos un nodo y nos devuelve una lista de vecinos con sus pesos
    {

        List<Tuple<Nodo, float>> weightN = new List<Tuple<Nodo, float>>();//Del nodo se recorre los vecinos y por cada vecino agrega una tupla que contiene el vencino y el peso

        foreach (var item in n.neighbours)//lista de vecinos
        {
            weightN.Add(Tuple.Create(item, Vector3.Distance(n.position, item.position)));//agrego una tupla por cada nodo, que contiene sus vecinos y costo tentativo
        }

        return weightN;
    }

    //Heuristic
    public float heuristic(Nodo n)
    {
        float heuristicroute = Vector3.Distance(n.position, End.position);

        return heuristicroute;
    }


   //ThetaStar Functions

   //Multi Line Of Sight
    public bool insigth(Nodo n, Nodo g)
    {
        //Retornará Falso si no ve a alguno de los neighbours

        RaycastHit hitInfo;

        var range = Vector3.Distance(g.position, n.position);
        foreach (var item in n.neighbours)
        {
            if (Physics.Raycast(n.position, g.position - n.position, out hitInfo, range, visibles))
            {
                // TO DO: FIX THIS

                //Esto da error porque el nodo no se puede comparar el uno con el otro usando transforms.
                if (hitInfo.Equals(g)) return false;
            }
        }
        return true;

    }

    public float cost(Nodo n, Nodo g)
    {
        float fathershipcost = Vector3.Distance(g.position, n.position);

        return fathershipcost;
    }

}