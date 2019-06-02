using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        transform.position += (current.transform.position - transform.position).normalized * speed * Time.deltaTime;

        if(Vector3.Distance(transform.position, current.transform.position) < threshold)
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
