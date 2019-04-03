using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverCollision : MonoBehaviour
{
    public GameObject ball;
    public Rigidbody ballrb;
    public GameObject lever;
    public Rigidbody leverrb;
    public Vector3 contact;
    // Start is called before the first frame update
    void Start()
    {
        leverrb = lever.GetComponent<Rigidbody>();
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballrb = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionExit(Collision col)
    {
        //get contact point in worldspace from ball
        //find closest local point on contacted lever to that worldspace point
        //calculate velocity of lever at that local point
        //Add that velocity to the ball
        //Debug.Log("Lever Point = " + leverrb.gameObject.GetComponent<Collider>().ClosestPointOnBounds(lever.transform.position));
        //Debug.Log("Lever Point = " + leverrb.gameObject.GetComponent<Collider>().ClosestPointOnBounds(ball.transform.position));
        contact = leverrb.gameObject.GetComponent<Collider>().ClosestPointOnBounds(ball.transform.position);
        Debug.Log("Velocity = " + leverrb.GetPointVelocity(transform.TransformPoint(contact)));
        //Debug.Log(leverrb.GetComponent<Collider>().bounds.Contains(contact));
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(contact, 0.01f);
    }
}
