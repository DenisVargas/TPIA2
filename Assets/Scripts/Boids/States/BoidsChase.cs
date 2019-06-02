using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class BoidsChase<T> : State<T>
{
    public Transform transform;
    public Transform target;
    public Boids boids;

    public BoidsChase(Transform mytrans, Transform myTarget, Boids myBoids)
    {
        transform = mytrans;
        target = myTarget;
        boids = myBoids;
    }

    public override void Enter()
    {
        //Debug.Log("En FlockingChase Enter target es: " + target);
        boids.m_Material.color = Color.red;
    }

    public override void Update()
    {
        if (boids.lineOfSight == true)
        {
            transform.LookAt(target);
            boids.agent.Steer((target.transform.position - transform.position).normalized);
        }
        else
        { boids.stateMachine.Feed(Feed2.IsNotInSigthP); }

        //Debug.Log("En FlockingChase Update target es: " + target);
    }

    public override void Exit()
    {
        // Debug.Log("En FlockingChase Exit target es: " + target);
    }

}*/
