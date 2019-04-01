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
    public Toggle randomRotationInversionToggle;
    public Toggle invertGravityToggle;
    public Toggle horizontalMode;
    public Toggle neonNightMode;


    void Start()
    {

        infiniteModeToggle.isOn = (PlayerPrefs.GetString("isModeInfinite", "false") == "true") ? true : false;
        invertedControlsToggle.isOn = (PlayerPrefs.GetString("isControlInverted", "false") == "true") ? true : false;
        randomRotationSpeedToggle.isOn = (PlayerPrefs.GetString("isLeverSpeedRandom", "false") == "true") ? true : false;
        farLeverSpacingToggle.isOn = (PlayerPrefs.GetString("isSpacingFar", "false") == "true") ? true : false;
        bigBounceToggle.isOn = (PlayerPrefs.GetString("isBouncy", "false") == "true") ? true : false;
        randomRotationInversionToggle.isOn = (PlayerPrefs.GetString("isControlRandom", "false") == "true") ? true : false;
        invertGravityToggle.isOn = (PlayerPrefs.GetString("isGravityInverted", "false") == "true") ? true : false;
        horizontalMode.isOn = (PlayerPrefs.GetString("isHorizontalMode", "false") == "true") ? true : false;
        neonNightMode.isOn = (PlayerPrefs.GetString("isNightMode", "false") == "true") ? true : false;
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
    public void SetRandomRotationInversionToggle(bool isToggled)
    {
        StoreToggleBool("isControlRandom", isToggled);
    }
    public void SetInvertGravityToggle(bool isToggled)
    {
        StoreToggleBool("isGravityInverted", isToggled);
    }
    public void SetHorizontalModeToggle(bool isToggled)
    {
        StoreToggleBool("isHorizontalMode", isToggled);
    }
    public void SetNeonNightModeToggle(bool isToggled)
    {
        StoreToggleBool("isNightMode", isToggled);
    }

    void Update()
    {
    }
}
