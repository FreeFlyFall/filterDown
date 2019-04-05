using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    // Lights//move to scene handler
    public Light directionalLight;
    public Material nightSkybox;
    public StateManager state;

    void Start()
    {
        if (state.isNightMode == "true")
        {
            ///move these to env handler
            directionalLight.enabled = false;
            RenderSettings.skybox = nightSkybox;
        }
    }
}
