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

    // Lever related variable declaration and definition
    public GameObject dayModeLeverPrefab;
/// Add new nightMode levers and access from scriptable object
    public GameObject nightModeLeverGreenPrefab;
    public GameObject nightModeLeverBluePrefab;
    public GameObject nightModeLeverRedPrefab;


    private GameObject leverPrefab;
    private GameObject[] leverArray; // = new GameObject[5];
    private Vector3 leverPos;
    private Quaternion leverRot = Quaternion.Euler(0, 0, 0);
    private int numberOfLevers;
    private bool isColumn1 = true;

    // Lever spacings
    private int regSpacing;
    private int farSpacing;

    // Array for assigning random rotation speeds dynamically
    private float[] rotationSpeedArray; // = new float[5];

    // Array for assigning random rotation inversion dynamically
    private Vector3[] controlArray; // = new Vector3[5];

    // Buffer for lever deletion based on ball positioning
    private float leverDeleteBuffer = 10;

    // bool for determining column for pinball mode controls
    public bool isLeftPin = true;

    //Return random from within custom range for lever rotation speed
    private float GetRandomLeverSpeed()
    {
        return Random.Range(80, 230);
    }

    void Start()
    {
        state.SetBools();
        // Clear top score on start for testing
        //PlayerPrefs.DeleteKey("topInfiniteScore");

        // Set physics, lever positioning, and spacing per gravity mode
        if (state.isGravityInverted == "true")
        {
            leverPos = new Vector3(-2, 4, 0);
            Physics.gravity = new Vector3(0, 9.81f, 0);
            regSpacing = -4;
            farSpacing = -6;
        }
        else
        {
            leverPos = new Vector3(-2, 0, 0);
            Physics.gravity = new Vector3(0, -9.81f, 0);
            regSpacing = 4;
            farSpacing = 6;
        }

        ///Separate
        // Use different prefab for lever per mode
        if (state.isNightModeGreen == "true")
        {
            leverPrefab = nightModeLeverGreenPrefab;
        }
        else if (state.isNightModeBlue == "true")
        {
            leverPrefab = nightModeLeverBluePrefab;
        }
        else if (state.isNightModeRed == "true")
        {
            leverPrefab = nightModeLeverRedPrefab;
        }
        else
        {
            // lever for regular mode.
            leverPrefab = dayModeLeverPrefab;
        }

//        // Set conditional based on boolean manager
//        if (state.isModeInfinite == "true") { numberOfLevers = 1000; }
///// set to gamemode specific number once finite mode is implemented
//        else { numberOfLevers = 1000; } 

        if (state.isHorizontalMode == "true")
        {
            numberOfLevers = 350;
            leverArray = new GameObject[350];
            controlArray = new Vector3[350];
            rotationSpeedArray = new float[350];
        } else
        {
            leverArray = new GameObject[10];
            controlArray = new Vector3[10];
            rotationSpeedArray = new float[10];
            numberOfLevers = 10;
        }

        PlaceInitialLevers();

        // Start checking conditionals for lever state changes
        StartCoroutine(CheckNextLever());
    }

    // Initialize the first levers in the scene
    private void PlaceInitialLevers()
    {
        for (int i = 0; i < numberOfLevers; i++)
        {
            GameObject newLever = Instantiate(leverPrefab) as GameObject;
            newLever.transform.position = leverPos;
            newLever.transform.rotation = leverRot;
            leverArray[i] = newLever;
            rotationSpeedArray[i] = GetRandomLeverSpeed();

            if (state.isPinballControl == "true")
            {
                if (isLeftPin == true)
                {
                    controlArray[i] = input.rotationPref;
                    isLeftPin = false;
                } else
                {
                    controlArray[i] = -input.rotationPref;
                    isLeftPin = true;
                }
            }
            if (state.isControlRandom == "true")
            {
                controlArray[i] = Random.Range(0f, 1f) >= 0.5f ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
            }

            SetLeverSpacingAndColumn();
            leverRot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)); //(0, 0, -45); for -1 slope testing
        }
    }

    public void Update()
    {
        // Rotate each lever in the array for the initial levers according to settings
        for (int i = 0; i < leverArray.Length; i++)
        {
            float rotationSpeedArrayIndex = rotationSpeedArray[i];
            Vector3 controlArrayIndex = controlArray[i];

            // According to player preferences, and the current mode, use either rotation or torque methods to rotate the lever
            if (state.isControlRandom == "true" && state.isLeverSpeedRandom == "true" || state.isLeverSpeedRandom == "true" && state.isPinballControl == "true")
            {
                if(state.isEasyMode == "true")
                {
                    leverArray[i].transform.Rotate(controlArrayIndex * input.rotationInput * rotationSpeedArrayIndex * Time.deltaTime);
                } else
                {
                    leverArray[i].GetComponent<Rigidbody>().AddTorque(controlArrayIndex * input.rotationInput * rotationSpeedArrayIndex * input.torqueModeMultiplier * Time.deltaTime);
                }
            }
            else if (state.isControlRandom == "true" || state.isPinballControl == "true")
            {
                if(state.isEasyMode == "true")
                {
                    leverArray[i].transform.Rotate(controlArrayIndex * input.rotationInput * input.rotationSpeed * Time.deltaTime);
                } else
                {
                    leverArray[i].GetComponent<Rigidbody>().AddTorque(controlArrayIndex * input.rotationInput * input.rotationSpeed * Time.deltaTime);
                }
            }
            else if (state.isLeverSpeedRandom == "true")
            {
                if(state.isEasyMode == "true")
                {
                    leverArray[i].transform.Rotate(input.rotationPref * input.rotationInput * rotationSpeedArrayIndex * Time.deltaTime);
                } else
                {
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

    // Generate new levers if conditions are met
    IEnumerator CheckNextLever()
    {
        if(state.isHorizontalMode == "true")
        {
///edit distance buffer later specifically for horizontal mode (10)
            if(leverArray[0].transform.position.x < ball.transform.position.x - (leverDeleteBuffer + 10))
            {
                SetupNextLever();
            }            
        }
        else if (state.isGravityInverted == "true")
        {
            if (leverArray[0].transform.position.y < ball.transform.position.y - leverDeleteBuffer)
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
///Debug.Log("destroyed");
        // Move levers and their properties down an index in the array
        for (int i = 0; i < leverArray.Length - 1; i++)
        {
            leverArray[i] = leverArray[i + 1];
            rotationSpeedArray[i] = rotationSpeedArray[i + 1];
            controlArray[i] = controlArray[i + 1];
        }
        // Create new lever at end of array with proper settings
        leverRot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)); //(0, 0, -45); for -1 slope testing
        GameObject newLever = Instantiate(leverPrefab) as GameObject;
        newLever.transform.position = leverPos;
        newLever.transform.rotation = leverRot;
        leverArray[leverArray.Length - 1] = newLever;
        rotationSpeedArray[rotationSpeedArray.Length - 1] = GetRandomLeverSpeed();
        if (state.isPinballControl == "true")
        {
            if (isLeftPin == true)
            {
                controlArray[controlArray.Length - 1] = input.rotationPref;
                isLeftPin = false;
            }
            else
            {
                controlArray[controlArray.Length - 1] = -input.rotationPref;
                isLeftPin = true;
            }
        }
        if (state.isControlRandom == "true")
        {
            controlArray[controlArray.Length - 1] = Random.Range(0f, 1f) >= 0.5f ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
        }
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
