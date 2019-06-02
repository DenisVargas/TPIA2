using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
*   Esta clase es una unidad que se mueve en direcciones aleatorias mientras transcurre el tiempo.
 */

public class SteeredAgent : MonoBehaviour
{
    private Rigidbody rb;
    public float force;

    public Vector3 Velocity
    {
        get { return rb.velocity; }
    }

    public Vector3 Position
    {
        get { return rb.position; }
    }

    public Vector3 Direction
    {
        get { return transform.forward; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //transform.forward = new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f)).normalized;
    }

    public void Steer(Vector3 dir)
    {
        rb.AddForce(dir * force);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(Velocity.x, 0, Velocity.z);

        if (Velocity.magnitude >= 0.01f)
            transform.forward = Velocity;
    }
}

