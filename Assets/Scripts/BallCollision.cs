using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public GameObject ball;
    public Rigidbody ballrb;
    public GameObject lever;
    public Rigidbody leverrb;
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
        //calculate velocity of lever at that local point // can't because I'm rotating it instead of adding torque
        //Add that velocity to the ball

        // oooooooh. Maybe this wouldn't even be an issue if I used torque to rotate the levers
        // I was wondering why unity wouldn't have included momentum from rigidbody contacts intheir engine
        // I think it's just that I'm rotating the transform which doesn't create any velocity in the lever
        // to be transfered to the ball as momentum. ooooooh.
        // Now I'm going to have to rewrite the lever controls and hope they still work well.
        // I was thinking that the ball would have too much of an effect on the levers, and I froze the z axis rotation
        // for the rigid body. just increase the mass.

        //Vector3 contact = ballrb.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        //Debug.Log("Ball Point = " + contact);
        //Debug.Log("Velocity = " + leverrb.GetPointVelocity(transform.TransformPoint(contact)));
    }

}
