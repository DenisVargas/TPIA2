using System.Collections.Generic;
using UnityEngine;
using IA.PathFinding;

public class WalkerSimple : MonoBehaviour
{
    public List<Nodo> waypoints;
    public float threshold;
    public float speed;
    public bool loop;
    
    private int currentIndex;
    private bool finished;


    private void Update()
    {
        if (finished) return;

        var current = waypoints[currentIndex];
        transform.position += (current.position - transform.position).normalized * speed * Time.deltaTime;

        if(Vector3.Distance(transform.position, current.position) < threshold)
        {
            currentIndex++;
            if(currentIndex == waypoints.Count)
            {
                if (loop) currentIndex = 0;
                else finished = true;
            }
        }
    }

    // private void OnDrawGizmos()
    // {
    //     if (waypoints == null || waypoints.Count <= 1) return;
    //     Gizmos.color = Color.red;       
    // }
}
