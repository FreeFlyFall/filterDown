using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{

    private float rotationSpeed = 130f;
    private float rotationInput;

    void Update()
    {
        rotationInput = UnityEngine.Input.GetAxis("Horizontal");
        transform.Rotate(-Vector3.forward * rotationInput * rotationSpeed * Time.deltaTime);
    }
}