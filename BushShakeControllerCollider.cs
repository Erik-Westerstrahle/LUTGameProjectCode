using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushShakeControllerCollider : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure the player has the "Player" tag
        {
            Debug.Log("Player entered the bush trigger zone.");
            // Start shaking
            isShaking = true;
            currentShakeTime = shakeDuration;
            bushMaterial.SetFloat("_ShakeMagnitude", shakeMagnitude);  // Trigger the shake
            Debug.Log($"Shaking started with magnitude {shakeMagnitude} for duration {shakeDuration}.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the bush trigger zone.");
            // Optionally stop shaking immediately when the player leaves
            isShaking = false;
            bushMaterial.SetFloat("_ShakeMagnitude", 0f);  // Stop the shaking
            Debug.Log("Shaking stopped due to player exit.");
        }
    }
}