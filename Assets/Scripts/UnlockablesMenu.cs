﻿using System.Collections;
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

    public Toggle neonNightModeGreen;
    public Toggle neonNightModeBlue;
    public Toggle neonNightModeRed;

    public Toggle easyMode;
    public Toggle PinballMode;


    void Start()
    {
        infiniteModeToggle.isOn = (PlayerPrefs.GetString("isModeInfinite", "true") == "true") ? true : false;
        invertedControlsToggle.isOn = (PlayerPrefs.GetString("isControlInverted", "false") == "true") ? true : false;
        randomRotationSpeedToggle.isOn = (PlayerPrefs.GetString("isLeverSpeedRandom", "false") == "true") ? true : false;
        farLeverSpacingToggle.isOn = (PlayerPrefs.GetString("isSpacingFar", "false") == "true") ? true : false;
        bigBounceToggle.isOn = (PlayerPrefs.GetString("isBouncy", "false") == "true") ? true : false;
        randomRotationInversionToggle.isOn = (PlayerPrefs.GetString("isControlRandom", "false") == "true") ? true : false;
        invertGravityToggle.isOn = (PlayerPrefs.GetString("isGravityInverted", "false") == "true") ? true : false;
        horizontalMode.isOn = (PlayerPrefs.GetString("isHorizontalMode", "false") == "true") ? true : false;

        neonNightModeGreen.isOn = (PlayerPrefs.GetString("isNightModeGreen", "false") == "true") ? true : false;
        neonNightModeBlue.isOn = (PlayerPrefs.GetString("isNightModeBlue", "false") == "true") ? true : false;
        neonNightModeRed.isOn = (PlayerPrefs.GetString("isNightModeRed", "false") == "true") ? true : false;

        easyMode.isOn = (PlayerPrefs.GetString("isEasyMode", "false") == "true") ? true : false;
        PinballMode.isOn = (PlayerPrefs.GetString("isPinballControl", "false") == "true") ? true : false;
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

    // Night modes
    public void SetNeonNightModeGreenToggle(bool isToggled)
    {
        StoreToggleBool("isNightModeGreen", isToggled);
    }
    public void SetNeonNightModeBlueToggle(bool isToggled)
    {
        StoreToggleBool("isNightModeBlue", isToggled);
    }
    public void SetNeonNightModeRedToggle(bool isToggled)
    {
        StoreToggleBool("isNightModeRed", isToggled);
    }


    public void SetEasyModeToggle(bool isToggled)
    {
        StoreToggleBool("isEasyMode", isToggled);
    }
    public void SetPinballModeToggle(bool isToggled)
    {
        StoreToggleBool("isPinballControl", isToggled);
    }
}
