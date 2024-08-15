using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushShakeDistance : MonoBehaviour
{
       public Material bushMaterial;
    public Transform playerTransform;
    public float maxShakeDistance = 5f;

    void Update()
    {
        Vector3 playerPosition = playerTransform.position;
        bushMaterial.SetVector("_PlayerPosition", playerPosition);

        // Optionally control the shake distance dynamically
        float distance = Vector3.Distance(transform.position, playerPosition);
        float shakeMagnitude = Mathf.Clamp01(1 - (distance / maxShakeDistance));
        bushMaterial.SetFloat("_ShakeMagnitude", shakeMagnitude);
    }
}
