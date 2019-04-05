using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public StateManager state;
    public ScoreSO scoreOB;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Vector2 initialPosition;
    //public Material dayModeColorMaterial;
    public Material nightModeColorMaterial;
    public PhysicMaterial physicMaterial;

    void Start()
    {
        initialPosition = new Vector2(transform.position.x, transform.position.y);
        rb = GetComponent<Rigidbody>();

        // Add small force at game start to prevent physics bug when stopping on level lever
        rb.AddForce(transform.right, ForceMode.Impulse);

        // Set Bouncy mode varible
        if (state.isBouncy == "true")
        {
            physicMaterial.bounciness = 0.8f;
        }
        else
        {
            physicMaterial.bounciness = 0.45f;
        }

        // Set ball color for night mode
        if (state.isNightMode == "true")
        {
            GetComponent<MeshRenderer>().material = nightModeColorMaterial;
        } else
        {
            // default ball color is already set in scene
        }

        // Start checking for out of bounds
        StartCoroutine(CheckBounds());
        StartCoroutine(ChangeScore());
    }

    IEnumerator CheckBounds()
    {
        if ((transform.position.x > 4.0f || transform.position.x < -4.0f) && state.isHorizontalMode != "true")
        {
            state.SaveAndReloadScene();
        }
        else if (state.isHorizontalMode == "true")
        {
            if (state.isGravityInverted != "true")
            {
                if (transform.position.y < -transform.position.x - 5.0f)
                {
                    state.SaveAndReloadScene();
                }
            }
            else
            {
                //Change level setup based on booleans to allow for the same buffer
                if (transform.position.y > transform.position.x + 8.5f)
                {
                    state.SaveAndReloadScene();
                }
            }
        }
            
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(CheckBounds());
    }

    IEnumerator ChangeScore()
    {
        if (state.isHorizontalMode == "true")
        {
            scoreOB.score = (transform.position.x - initialPosition.x).ToString("0");
        }
        else if (state.isGravityInverted == "true")
        {

            scoreOB.score = (transform.position.y - initialPosition.y).ToString("0");
        }
        else
        {
            scoreOB.score = (-transform.position.y + initialPosition.y).ToString("0");
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ChangeScore());
    }
}
