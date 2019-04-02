using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class CreateLevers : MonoBehaviour
{
    public StateManager state;

    //Lights
    public Light directionalLight;

    //Ball properties
    public GameObject ball;
    private Rigidbody ballrb;
    private Vector2 initialBallPosition;
    public Material dayModeBallColorMaterial;
    public Material nightModeBallColorMaterial;
    private Material ballColorMaterial;

    //Lever properties
    public GameObject dayModeLeverPrefab;
    public GameObject nightModeLeverPrefab;
    private GameObject leverPrefab;
    private GameObject[] leverArray = new GameObject[5];

    private Vector3 leverPos;
    private Quaternion leverRot = Quaternion.Euler(0, 0, 0);

    private int numberOfLevers;
    private bool isColumn1 = true;

    //Score properties
    public TextMeshProUGUI infiniteScoreText;
    [SerializeField] private ScoreSO scoreOB;
    public TextMeshProUGUI topInfiniteScoreText;

    //GameSettings
    private string isModeInfinite;

    private string isSpacingFar;
    private int regSpacing;
    private int farSpacing;

    private string isBouncy;
    public PhysicMaterial ballPhysicMaterial;

    private float rotationSpeed;
    private float[] rotationspeedArray = new float[5];
    private string isLeverSpeedRandom;
    private float rotationInput;
    // Rotation Controls
    private string isControlInverted;
    private string isControlRandom;
    private Vector3[] randomControlArray = new Vector3[5];
    private Vector3 rotationPref;
    private bool isInputFromMobile = false;
    // Variables for mobile input calculations
    static float lerpUpT = 0.0f;
    static float lerpDownT = 0.0f;
    private float lastPressedMobileInputValue = 0.0f;
    private float customInputGravity = 7.0f;

    private string isGravityInverted;
    private float leverDeleteBuffer;

    private string isHorizontalMode;

    private string isNightMode;// = "true";
    public Material nightSkybox;

    //Return custom range for lever rotation speed
    private float GetRandomLeverSpeed()
    {
        return Random.Range(80, 230);
    }

    public void SetSens()
    {
        rotationSpeed = PlayerPrefs.GetFloat("rotationSpeed", 130f);
    }

    void Start()
    {
        SetSens();

        //Set Unlockable Bools
        isModeInfinite = PlayerPrefs.GetString("isModeInfinite", "true");

        isControlInverted = PlayerPrefs.GetString("isControlInverted", "false");
        rotationPref = isControlInverted == "true" ? Vector3.forward : -Vector3.forward;
        isControlRandom = PlayerPrefs.GetString("isControlRandom", "false");

        isLeverSpeedRandom = PlayerPrefs.GetString("isLeverSpeedRandom", "false");

        isSpacingFar = PlayerPrefs.GetString("isSpacingFar", "false");

        isBouncy = PlayerPrefs.GetString("isBouncy", "false");
        if (isBouncy == "true")
        {
            ballPhysicMaterial.bounciness = 1f;
        }
        else
        {
            ballPhysicMaterial.bounciness = 0.5f;
        }

        isGravityInverted = PlayerPrefs.GetString("isGravityInverted", "false");
        if (isGravityInverted == "true")
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

        isHorizontalMode = PlayerPrefs.GetString("isHorizontalMode", "false");

        isNightMode = PlayerPrefs.GetString("isNightMode", "false");
        if (isNightMode == "true")
        {
            leverPrefab = nightModeLeverPrefab;
            // Set ball color
            ballColorMaterial = nightModeBallColorMaterial;
            ball.GetComponent<MeshRenderer>().material = ballColorMaterial;
            directionalLight.enabled = false;
            RenderSettings.skybox = nightSkybox;
            
        } else
        {
            leverPrefab = dayModeLeverPrefab;
            /// Leave default skybox; // inaccessible?
            /// No need to set daytime parameters since they're default
        }

        // end unlockable bools

        //Clear top score for testing
        //PlayerPrefs.DeleteKey("topInfiniteScore");

        // Set score UI
        scoreOB.topScore = PlayerPrefs.GetString("topInfiniteScore", "0");
        topInfiniteScoreText.SetText("Top score: " + scoreOB.topScore);

        // Initialize score scriptable object score
        scoreOB.score = "0";

        //Set conditional based on boolean manager
        if (isModeInfinite == "true") { numberOfLevers = 5; }
        else { numberOfLevers = 5; } /// set to gamemode specific number later from separate script.

        initialBallPosition = new Vector2(ball.transform.position.x, ball.transform.position.y);

        PlaceInitialLevers();

        // Add small force at game start to prevent physics bug when stopping on level lever
        ballrb = ball.GetComponent<Rigidbody>();
        ballrb.AddForce(transform.right * 2, ForceMode.Impulse);

        // Start tracking ininite game mode score
        StartCoroutine(SetInfiniteScoreText());
        // Start checking for out of bounds
        StartCoroutine(CheckBounds());
        // Start checking conditionals for lever state changes
        StartCoroutine(CheckNextLever());

        StartCoroutine(ChangeScore());
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
            if (isControlRandom == "true")
            {
                randomControlArray[i] = Random.Range(0f, 1f) >= 0.5f ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
            }
            SetLeverSpacingAndColumn();
            leverRot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
        }
    }

    public void Update()
    {
        /* Handle Desktop and mobile input to turn the levers with similar inertia.
        * Is there really no way to simulate buttonpresses for the engine inputs? Seriously?
        * The code has to be somewhere. At least we have the CPIM now. Where would I be without that?
        */
        rotationInput = Input.GetAxis("Horizontal");
        if (Application.platform == RuntimePlatform.Android)
        {
            if (CrossPlatformInputManager.GetButton("a") == true)
            {
                lerpDownT = 0f;
                SetCustomMobileInputVariables(-1f);
                lerpUpT += customInputGravity * Time.deltaTime;
            }
            else if (CrossPlatformInputManager.GetButton("d") == true)
            {
                lerpDownT = 0f;
                SetCustomMobileInputVariables(1f);
                lerpUpT += customInputGravity * Time.deltaTime;
            }
            if (CrossPlatformInputManager.GetButton("a") == false && CrossPlatformInputManager.GetButton("d") == false)
            {
                lerpUpT = 0f;
                rotationInput = Mathf.Lerp(lastPressedMobileInputValue, 0f, lerpDownT);
                lerpDownT += customInputGravity * Time.deltaTime;
            }
        }
        //rotate each lever in the array for the initial levers
        for (int i = 0; i < leverArray.Length; i++)
        {
            float rotationSpeedArrayIndex = rotationspeedArray[i];
            Vector3 randomControlArrayIndex = randomControlArray[i];
            if (isControlRandom == "true" && isLeverSpeedRandom == "true")
            {
                leverArray[i].transform.Rotate(randomControlArrayIndex * rotationInput * rotationSpeedArrayIndex * Time.deltaTime);
            }
            else if (isControlRandom == "true")
            {
                leverArray[i].transform.Rotate(randomControlArrayIndex * rotationInput * rotationSpeed * Time.deltaTime);
            }
            else if (isLeverSpeedRandom == "true")
            {
                leverArray[i].transform.Rotate(rotationPref * rotationInput * rotationSpeedArrayIndex * Time.deltaTime);
            }
            else
            {
                leverArray[i].transform.Rotate(rotationPref * rotationInput * rotationSpeed * Time.deltaTime);
            }
        }
    }

    void SetCustomMobileInputVariables(float horizontalInput)
    {
        rotationInput = Mathf.Lerp(0.0f, horizontalInput, lerpUpT);
        lastPressedMobileInputValue = horizontalInput;
    }

    IEnumerator SetInfiniteScoreText()
    {
        infiniteScoreText.SetText("Score: " + scoreOB.score);
        if (int.Parse(scoreOB.score) > int.Parse(scoreOB.topScore))
        {
            topInfiniteScoreText.SetText("Top score: " + scoreOB.score);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetInfiniteScoreText());
    }

    IEnumerator ChangeScore()
    {
        if (isHorizontalMode == "true")
        {
            scoreOB.score = (ball.transform.position.x - initialBallPosition.x).ToString("0");
        }
        else if (isGravityInverted == "true")
        {

            scoreOB.score = (ball.transform.position.y - initialBallPosition.y).ToString("0");
        }
        else
        {
            scoreOB.score = (-ball.transform.position.y + initialBallPosition.y).ToString("0");
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ChangeScore());
    }

    // Create new data for lever positioning
    IEnumerator CheckNextLever()
    {
        if(isHorizontalMode == "true")
        {
            //edit buffer later (10)
            if(leverArray[0].transform.position.x < ball.transform.position.x - 10)
            {
                SetupNextLever();
            }
        }
        else if (isGravityInverted == "true")
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
        else if (isHorizontalMode == "true")
        {
            leverPos.x += 4;
        }
        else
        {
            leverPos.x -= 4;
        }
        isColumn1 = !isColumn1;
        if (isSpacingFar == "true") { leverPos.y -= farSpacing; }
        else { leverPos.y -= regSpacing; }
    }

    IEnumerator CheckBounds()
    {
        if ((ball.transform.position.x > 5 || ball.transform.position.x < -5) && isHorizontalMode != "true")
        {
            state.SaveAndReloadScene();
        }
        else if (isHorizontalMode == "true")
        {
            if (isGravityInverted != "true")
            {
                if (ball.transform.position.y < leverArray[leverArray.Length - 1].transform.position.y)
                {
                    state.SaveAndReloadScene();
                }
            }
            else
            {
                if (ball.transform.position.y > leverArray[leverArray.Length - 1].transform.position.y)
                {
                    state.SaveAndReloadScene();
                }
            }
        }
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(CheckBounds());
    }
}
