using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Light directionalLight;
    public Material nightSkybox;
    public StateManager state;

    void Start()
    {
        if (state.isNightModeGreen == "true" || state.isNightModeBlue == "true" || state.isNightModeRed == "true")
        {
            directionalLight.enabled = false;
            RenderSettings.skybox = nightSkybox;
        }
    }
}
