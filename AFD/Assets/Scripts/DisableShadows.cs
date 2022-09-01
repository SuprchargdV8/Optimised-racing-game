using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSubCamShadows : MonoBehaviour
{
    // either drag this in via the inspector
    public Camera mainCamera;

    private ShadowQuality originalShadowSettings;

    // or get it on runtime
    private void Awake()
    {
        // mainCamera will be the one attached to this object
        mainCamera = GetComponent<Camera>();

        // store original shadow settings
        originalShadowSettings = QualitySettings.shadows;
    }

    // callback before ANY camera starts rendering
    private void onPreRender(Camera cam)
    {
        // if mainCamera set to originalShadowSettings 
        // could also simply return but just to be sure
        //
        // for other camera disable shadows
        QualitySettings.shadows = cam == mainCamera ? originalShadowSettings : ShadowQuality.Disable;   
    }

    // callback after ANY camera finishes rendering
    private void onPostRenderer(Camera cam)
    {
        // restore shadow settings
        QualitySettings.shadows = originalShadowSettings;
    }
}