﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Make sure the camera follow target's rigidbody interpolation is set to "interpolate" in the inspector
    public Transform target; 

    private float smoothSpeed = 0.4f;
    public Vector3 offset;
    private Vector3 zeroVelocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref zeroVelocity, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
