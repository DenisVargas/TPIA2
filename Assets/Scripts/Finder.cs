using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA.PathFinding;

//La utilidad de Finder es Setear a un Walker su camino
public class Finder : MonoBehaviour
{
    public Nodo start;
    public Nodo End;
    public WalkerSimple walker;

    //FieldOfView
    //Nota: Esto es usado en FinderThetaStar
    public float VisibilityRange;
    public LayerMask visibles = ~0;

    private void Start()
    {      
        //Aca va a cambiar el método.
        var path = BFS.Run(start, satisfies, Expand);
        var path = DFS.Run(start, satisfies, Expand);
        var path = Dijkstra.Run(start, satisfies, ExpandWeighted);
        var path = AStar.Run(start, satisfies, ExpandWeighted, heuristic);
        var path = ThetaStar.Run(start, satisfies, ExpandWeighted, heuristic, insigth, cost);

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
            weightN.Add(Tuple.Create(item, Vector3.Distance(n.transform.position, item.transform.position)));//agrego una tupla por cada nodo, que contiene sus vecinos y costo tentativo
        }

        return weightN;
    }

    //Heuristic
    public float heuristic(Nodo n)
    {
        float heuristicroute = Vector3.Distance(n.transform.position, End.transform.position);

        return heuristicroute;
    }


   //ThetaStar

   //Multi Line Of Sight
    public bool insigth(Nodo n, Nodo g)
    {
        //Retornará Falso si no ve a alguno de los neighbours

        RaycastHit hitInfo;

        range = Vector3.Distance(g.transform.position, n.transform.position);
        foreach (var item in n.neighbours)
        {
            if (Physics.Raycast(n.transform.position, g.transform.position - n.transform.position, out hitInfo, range, visibles))
            {
                if (hitInfo.transform != g.transform) return false;
            }
        }
        return true;

    }

    public float cost(Nodo n, Nodo g)
    {
        float fathershipcost = Vector3.Distance(g.transform.position, n.transform.position);

        return fathershipcost;
    }

}