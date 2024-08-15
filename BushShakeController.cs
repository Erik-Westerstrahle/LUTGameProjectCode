using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushShakeController : MonoBehaviour
{
    public Material bushMaterial;  // The material using the Shader Graph
    public float shakeDuration = 1.0f;  // How long the bush shakes
    public float shakeMagnitude = 0.1f; // The intensity of the shake

    private float currentShakeTime = 0f;
    private bool isShaking = false;

    void Update()
    {
        if (isShaking)
        {
            Debug.Log("Bush is shaking...");
            currentShakeTime -= Time.deltaTime;
            if (currentShakeTime <= 0f)
            {
                Debug.Log("Shaking stopped due to time elapse.");
                // Stop the shake
                isShaking = false;
                bushMaterial.SetFloat("_ShakeMagnitude", 0f);  // Stop the shaking
            }
        }
    }

    public void StartShaking()
    {
        Debug.Log("StartShaking called on BushShakeController.");
        isShaking = true;
        currentShakeTime = shakeDuration;
        bushMaterial.SetFloat("_ShakeMagnitude", shakeMagnitude);  // Trigger the shake
        Debug.Log($"Shaking started with magnitude {shakeMagnitude} for duration {shakeDuration}.");
    }

    public void StopShaking()
    {
        Debug.Log("StopShaking called on BushShakeController.");
        isShaking = false;
        bushMaterial.SetFloat("_ShakeMagnitude", 0f);  // Stop the shaking
        Debug.Log("Shaking stopped.");
    }
}
