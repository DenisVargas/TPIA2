using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA.LineOfSight;

public class Lider : MonoBehaviour
{
    public Transform enemy;
    public LayerMask myViewMask = ~0;

    public float range;
    public float viewAngle;

	// Use this for initialization
	void Start ()
    {
        var Sight = transform.CreateSightEntity(10, 60).setUniqueTarget(enemy);

        //Sight.visibles = myViewMask;

        print("The Enemy is in Sight:" + Sight.IsInSight());
	}

    void OnDrawGizmosSelected()
    {
        var position = transform.position;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(position, range);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(position, position + Quaternion.Euler(0, viewAngle, 0) * transform.forward * range);
        Gizmos.DrawLine(position, position + Quaternion.Euler(0, -viewAngle, 0) * transform.forward * range);

    }

}
