using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public Transform target;
    //public LineOfSight lineOfSight;
    public Material m_Material;

    //public FSM<Feed2> stateMachine;

    /*public BoidsPatrol<Feed2> patrol;
    public BoidsChase<Feed2> chase;
    public BoidsAttack<Feed2> attack;*/

    public LayerMask predatorMask;
    public LayerMask neighbourMask;

    public bool onSigth = true;

    public float cohesionRadius = 1f;
    public float separationRadius = 1f;
    public float alineationRadius = 1f;
    public float predatorRadius = 1f;

    public float cohesionWeight = 1f;
    public float separationWeight = 1f;
    public float alineationWeight = 1f;
    public float predatorWeight = 1f;

    public float callRadius;

    public SteeredAgent agent;

    private void Awake()
    {
        agent = GetComponent<SteeredAgent>();
    }

    void Start()
    {
        m_Material = GetComponent<Renderer>().material;

        /*patrol = new BoidsPatrol<Feed2>(transform, target, this);
        chase = new BoidsChase<Feed2>(transform, target, this);
        attack = new BoidsAttack<Feed2>(transform, target, this);

        patrol.AddTransition(Feed2.IsInSigthP, chase);

        chase.AddTransition(Feed2.IsNotInSigthP, patrol);
        chase.AddTransition(Feed2.TargetIsNear, attack);

        attack.AddTransition(Feed2.TargetIsNotNear, chase);

        stateMachine = new FSM<Feed2>(patrol);*/

    }



    public void FixedUpdate()
    {
        //stateMachine.Update();
        //onSigth = lineOfSight.IsInSight(target);

        CallDir();

        /*if (onSigth == true)
        {
            stateMachine.Feed(Feed2.IsInSigthP);
            Debug.DrawLine(transform.position, target.position);
            CallGroup();

        }*/

        //stateMachine.Feed(Feed2.IsNotInSigthP);


    }

    public void CallDir()
    {
        var cohesionDir = GetCohesionDir();
        var separationDir = GetSeparationDir();
        var alineationDir = GetAlineationDir();
        var predatorDir = GetPredatorDir();

        var totalWeights = cohesionWeight + separationWeight + alineationWeight + predatorWeight;

        var cohesionWFixed = cohesionWeight / totalWeights;
        var separationWFixed = separationWeight / totalWeights;
        var alineationWFixed = alineationWeight / totalWeights;
        var predatorWFixed = predatorWeight / totalWeights;

        var dir = cohesionDir * cohesionWFixed + separationDir * separationWFixed + alineationDir * alineationWFixed + predatorDir * predatorWFixed;

        agent.Steer(dir.normalized);
    }

    private Vector3 GetCohesionDir()
    {
        var cohesionColliders = Physics.OverlapSphere(transform.position, cohesionRadius, neighbourMask);
        if (cohesionColliders.Length == 0) return Vector3.zero;

        var acc = Vector3.zero;
        foreach (var collider in cohesionColliders)
        {
            acc += collider.transform.position;
        }

        var cohesionCenter = acc / cohesionColliders.Length;
        var dir = (cohesionCenter - agent.Position).normalized;

        return dir;
    }

    private Vector3 GetSeparationDir()
    {
        var separationColliders = Physics.OverlapSphere(transform.position, separationRadius, neighbourMask);
        if (separationColliders.Length == 0) return Vector3.zero;

        var acc = Vector3.zero;
        foreach (var collider in separationColliders)
        {
            acc += collider.transform.position;
        }

        var separationCenter = acc / separationColliders.Length;
        var dir = (separationCenter - agent.Position).normalized;

        return -dir;
    }

    private Vector3 GetAlineationDir()
    {
        var alineationColliders = Physics.OverlapSphere(transform.position, alineationRadius, neighbourMask);
        if (alineationColliders.Length == 0) return Vector3.zero;

        var acc = Vector3.zero;

        foreach (var collider in alineationColliders)
        {
            acc += collider.transform.forward;
        }

        var directionAverage = (acc / alineationColliders.Length).normalized;

        return directionAverage;
    }

    private Vector3 GetPredatorDir()
    {
        var predatorColliders = Physics.OverlapSphere(transform.position, predatorRadius, predatorMask);
        if (predatorColliders.Length == 0) return Vector3.zero;

        var acc = Vector3.zero;

        foreach (var collider in predatorColliders)
        {
            acc += collider.transform.position;
        }

        var predatorCenter = acc / predatorColliders.Length;

        var dir = (predatorCenter - agent.Position).normalized * (Vector3.Distance(predatorCenter, agent.Position) / predatorRadius);

        return -dir;
    }

    /// <summary>
    /// Si un objetivo es detectado, llama a todos los Voids y los suma a la persecución del objetivo.
    /// </summary>
    private void CallGroup()
    {
        Collider[] groupCall = new Collider[11];
        Boids Component;
        groupCall = Physics.OverlapSphere(transform.position, callRadius);
        foreach (var e in groupCall)
        {
            if (e == null) continue;
            Component = e.GetComponent<Boids>();
            if (Component != null)
                Component.JoinChase();
        }
    }

    public void JoinChase()
    {
        //stateMachine.Feed(Feed2.IsInSigthP);
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            m_Material.color = Color.black;
            print("ATACO");
            //stateMachine.Feed(Feed2.TargetIsNear);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, cohesionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, alineationRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, predatorRadius);
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, callRadius);
    }*/
}

/*public enum Feed2
{
    IsInSigthP,
    IsNotInSigthP,
    TargetIsNear,
    TargetIsNotNear

}*/
