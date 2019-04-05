using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour
{
    //Custom classes
    [SerializeField] private StateManager state;

    // Sensitivity
    [HideInInspector] public float rotationSpeed;

    // left and right rotaiotn input, -1 to +1, respectively
    [HideInInspector] public float rotationInput;

    // Rotation inversion preference in Vector3 form for proper multiplication
    [HideInInspector] public Vector3 rotationPref;

    // Variables for mobile input smoothing calculations
    static float lerpUpT = 0.0f;
    static float lerpDownT = 0.0f;
    private float lastPressedMobileInputValue = 0.0f;
    private float customInputGravity = 7.0f;
    [HideInInspector] public float torqueModeMultiplier = 50000.0f;
    // 3999999 - ~9999999 should be a good result for rotationSpeed

    void Start()
    {
        state.SetBools();
        SetSens();
        rotationPref = state.isControlInverted == "true" ? Vector3.forward : -Vector3.forward;
    }

    void Update()
    {
        //Handle input for Desktop
        rotationInput = Input.GetAxis("Horizontal");

        //Write over input for mobile
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
    }

    // Set player sensitivity based on PlayerPrefs and per mode
    public void SetSens()
    {
        if (state.isEasyMode != "true")
        {
            rotationSpeed = PlayerPrefs.GetFloat("rotationSpeed", 130f) * torqueModeMultiplier;
        }
        else
        {
            rotationSpeed = PlayerPrefs.GetFloat("rotationSpeed", 130f);
        }
    }

    // Abstracted for dryness
    void SetCustomMobileInputVariables(float horizontalInput)
    {
        rotationInput = Mathf.Lerp(0.0f, horizontalInput, lerpUpT);
        lastPressedMobileInputValue = horizontalInput;
    }
}
