using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu2 : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropDown;
    Resolution[] resolutions;
    public Slider sensSlider;
    public TextMeshProUGUI sensText;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> optionsList = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            optionsList.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(optionsList);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        GetandSetSensDisplay();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainMixerVolume", volume);
    }

    public void SetSens(float sens)
    {
        PlayerPrefs.SetFloat("rotationSpeed", sens);
        sensText.text = PlayerPrefs.GetFloat("rotationSpeed", 130f).ToString("0");
    }

    public void GetandSetSensDisplay()
    {
        sensSlider.value = PlayerPrefs.GetFloat("rotationSpeed", 130f);

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
