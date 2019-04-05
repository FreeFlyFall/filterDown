using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreateLevers : MonoBehaviour
{
    // Custom classes
    public StateManager state;
    public InputManager input;
    public Ball ball;

    // Lever dec&def
    public GameObject dayModeLeverPrefab;
    public GameObject nightModeLeverPrefab;
    private GameObject leverPrefab;
    private GameObject[] leverArray = new GameObject[8];
    private Vector3 leverPos;
    private Quaternion leverRot = Quaternion.Euler(0, 0, 0);
    private int numberOfLevers; // is set to 8 below
    private bool isColumn1 = true;

    // Lever spacings
    private int regSpacing;
    private int farSpacing;
    
    // Array for assigning random rotation speeds dynamically
    private float[] rotationspeedArray = new float[8];

    // Array for assigning random rotation inversion dynamically
    private Vector3[] randomControlArray = new Vector3[8];

    // Buffers for lever deletion based on ball positioning
    private float leverDeleteBuffer;

    //Return random from within custom range for lever rotation speed
    private float GetRandomLeverSpeed()
    {
        return Random.Range(80, 230);
    }

    void Start()
    {
        // Clear top score on start for testing
        //PlayerPrefs.DeleteKey("topInfiniteScore");

        if (state.isGravityInverted == "true")
        {
            leverPos = new Vector3(-2, 4, 0);
            Physics.gravity = new Vector3(0, 9.81f, 0);
            regSpacing = -4;
            farSpacing = -6;
            leverDeleteBuffer = -10.0f;
        }
        else
        {
            leverPos = new Vector3(-2, 0, 0);
            Physics.gravity = new Vector3(0, -9.81f, 0);
            regSpacing = 4;
            farSpacing = 6;
            leverDeleteBuffer = 10.0f;
        }

        if (state.isNightMode == "true")
        {
            leverPrefab = nightModeLeverPrefab;
        } else
        {
            // lever for regular mode.
            leverPrefab = dayModeLeverPrefab;
        }

        // Set conditional based on boolean manager
        if (state.isModeInfinite == "true") { numberOfLevers = 8; }
/// set to gamemode specific number later.
        else { numberOfLevers = 8; } 

        PlaceInitialLevers();

        // Start checking conditionals for lever state changes
        StartCoroutine(CheckNextLever());
    }



    // Initialize levers in scene
    private void PlaceInitialLevers()
    {
        for (int i = 0; i < numberOfLevers; i++)
        {
            GameObject newLever = Instantiate(leverPrefab) as GameObject;
            newLever.transform.position = leverPos;
            newLever.transform.rotation = leverRot;
            leverArray[i] = newLever;
            rotationspeedArray[i] = GetRandomLeverSpeed();
            if (state.isControlRandom == "true")
            {
                randomControlArray[i] = Random.Range(0f, 1f) >= 0.5f ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
            }
            SetLeverSpacingAndColumn();
            leverRot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
        }
    }

    public void Update()
    {
///Organize
        //rotate each lever in the array for the initial levers
        for (int i = 0; i < leverArray.Length; i++)
        {
            float rotationSpeedArrayIndex = rotationspeedArray[i];
            Vector3 randomControlArrayIndex = randomControlArray[i];
            if (state.isControlRandom == "true" && state.isLeverSpeedRandom == "true")
            {
                if(state.isEasyMode == "true")
                {
                    leverArray[i].transform.Rotate(randomControlArrayIndex * input.rotationInput * rotationSpeedArrayIndex * Time.deltaTime);
                } else
                {
                    // * by torqueModeMultiplier 
                    leverArray[i].GetComponent<Rigidbody>().AddTorque(randomControlArrayIndex * input.rotationInput * rotationSpeedArrayIndex * input.torqueModeMultiplier * Time.deltaTime);
                }
            }
            else if (state.isControlRandom == "true")
            {
                if(state.isEasyMode == "true")
                {
                    leverArray[i].transform.Rotate(randomControlArrayIndex * input.rotationInput * input.rotationSpeed * Time.deltaTime);
                } else
                {
                    leverArray[i].GetComponent<Rigidbody>().AddTorque(randomControlArrayIndex * input.rotationInput * input.rotationSpeed * Time.deltaTime);
                }
            }
            else if (state.isLeverSpeedRandom == "true")
            {
                if(state.isEasyMode == "true")
                {
                    leverArray[i].transform.Rotate(input.rotationPref * input.rotationInput * rotationSpeedArrayIndex * Time.deltaTime);
                } else
                {
                    // * by torqueModeMultiplier 
                    leverArray[i].GetComponent<Rigidbody>().AddTorque(input.rotationPref * input.rotationInput * rotationSpeedArrayIndex * input.torqueModeMultiplier * Time.deltaTime);
                }
            }
            else
            {
                if(state.isEasyMode == "true")
                {
                    leverArray[i].transform.Rotate(input.rotationPref * input.rotationInput * input.rotationSpeed * Time.deltaTime);
                }
                else
                {
                    leverArray[i].GetComponent<Rigidbody>().AddTorque(input.rotationPref * input.rotationInput * input.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    // Create new data for lever positioning
    IEnumerator CheckNextLever()
    {
        if(state.isHorizontalMode == "true")
        {
///edit distance buffer later (10)
            if(leverArray[0].transform.position.x < ball.transform.position.x - 10)
            {
                SetupNextLever();
            }
        }
        else if (state.isGravityInverted == "true")
        {
            if (leverArray[0].transform.position.y < ball.transform.position.y + leverDeleteBuffer)
            {
                SetupNextLever();
            }
        }
        else
        {
            if (leverArray[0].transform.position.y > ball.transform.position.y + leverDeleteBuffer)
            {
                SetupNextLever();
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CheckNextLever());
    }

    void SetupNextLever()
    {
        // Destroy first lever in array
        Destroy(leverArray[0]);
        // Move levers and their properties down an index in the array
        for (int i = 0; i < leverArray.Length - 1; i++)
        {
            leverArray[i] = leverArray[i + 1];
            rotationspeedArray[i] = rotationspeedArray[i + 1];
            randomControlArray[i] = randomControlArray[i + 1];
        }
        // Create new lever at end of array
        leverRot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
        GameObject newLever = Instantiate(leverPrefab) as GameObject;
        newLever.transform.position = leverPos;
        newLever.transform.rotation = leverRot;
        leverArray[leverArray.Length - 1] = newLever;
        rotationspeedArray[rotationspeedArray.Length - 1] = GetRandomLeverSpeed();
        randomControlArray[randomControlArray.Length - 1] = Random.Range(0f, 1f) >= 0.5f ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
        SetLeverSpacingAndColumn();
    }

    void SetLeverSpacingAndColumn()
    {
        if (isColumn1 == true) { leverPos.x += 4; }
        else if (state.isHorizontalMode == "true")
        {
            leverPos.x += 4;
        }
        else
        {
            leverPos.x -= 4;
        }
        isColumn1 = !isColumn1;
        if (state.isSpacingFar == "true") { leverPos.y -= farSpacing; }
        else { leverPos.y -= regSpacing; }
    }
}
