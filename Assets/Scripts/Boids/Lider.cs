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

    bool targetIsInSight;

	// Use this for initialization
	void Start ()
    {
        var Sight = transform.CreateSightEntity(10, 60).setUniqueTarget(enemy);

        //Sight.visibles = myViewMask;

        targetIsInSight = Sight.IsInSight();
        print("The Enemy is in Sight:" + targetIsInSight);
	}

#if (UNITY_EDITOR)
    void OnDrawGizmosSelected()
    {
        var position = transform.position;

        Gizmos.color = targetIsInSight ? Color.green : Color.red;
        Vector3 dirToTarget = (enemy.position - transform.position).normalized;
        Gizmos.DrawLine(transform.position, transform.position + dirToTarget * range);

        Gizmos.color = Color.white;
        Gizmos.matrix *= Matrix4x4.Scale(new Vector3(1,0,1));
        Gizmos.DrawWireSphere(position, range);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(position, position + Quaternion.Euler(0, viewAngle, 0) * transform.forward * range);
        Gizmos.DrawLine(position, position + Quaternion.Euler(0, -viewAngle, 0) * transform.forward * range);

    }
#endif

}
