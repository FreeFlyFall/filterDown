using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockablesMenu : MonoBehaviour
{
    public Toggle infiniteModeToggle;
    public Toggle invertedControlsToggle;
    public Toggle randomRotationSpeedToggle;
    public Toggle farLeverSpacingToggle;
    public Toggle bigBounceToggle;

    void Start()
    {
        infiniteModeToggle.isOn = (PlayerPrefs.GetString("isModeInfinite", "true") == "true") ? true : false;
        invertedControlsToggle.isOn = (PlayerPrefs.GetString("isControlInverted", "true") == "true") ? true : false;
        randomRotationSpeedToggle.isOn = (PlayerPrefs.GetString("isLeverSpeedRandom", "true") == "true") ? true : false;
        farLeverSpacingToggle.isOn = (PlayerPrefs.GetString("isSpacingFar", "true") == "true") ? true : false;
        bigBounceToggle.isOn = (PlayerPrefs.GetString("isBouncy", "true") == "true") ? true : false;
    }

    public void StoreToggleBool(string booleanName, bool isToggled)
    {
        string boolToString;
        boolToString = (isToggled == true) ? "true" : "false";
        //Debug.Log("before: " + booleanName + " = " + PlayerPrefs.GetString(booleanName, "null"));
        PlayerPrefs.SetString(booleanName, boolToString);
        //Debug.Log("after: " + booleanName + " = " + PlayerPrefs.GetString(booleanName, "null"));
    }

    public void SetInfiniteModeToggle(bool isToggled)
    {
        StoreToggleBool("isModeInfinite", isToggled);
    }
    public void SetInvertedControlsToggle(bool isToggled)
    {
        StoreToggleBool("isControlInverted", isToggled);
    }
    public void SetRandomRotationSpeedToggle(bool isToggled)
    {
        StoreToggleBool("isLeverSpeedRandom", isToggled);
    }
    public void SetFarLeverSpacingToggle(bool isToggled)
    {
        StoreToggleBool("isSpacingFar", isToggled);
    }
    public void SetBigBounceToggle(bool isToggled)
    {
        StoreToggleBool("isBouncy", isToggled);
    }
        

    void Update()
    {
    }
}
