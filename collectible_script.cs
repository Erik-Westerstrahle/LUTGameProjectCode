using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class collectible_script : MonoBehaviour
{
    public static event Action OnCollected;
    public static event Action OnAllCollected;

    public static int total;
    private static int collectedCount;

    private static bool levelInitialized = false;
    private static int previousTotal = 0; // Store the previous total to detect doubling

    void Awake()
    {
        Debug.Log("Collectible Awake called.");
        
    }

    void Start()
    {
        Debug.Log("Collectible OnEnable called.");

        if (!levelInitialized)
        {
            Debug.Log("Level not initialized. Resetting collectibles.");
            ResetCollectibles(); // Reset counts when the level starts
            levelInitialized = true; // Ensure this is only done once per level load
        }

     

        
        total++; // Increment total count for each collectible instantiated
        previousTotal = total; // Store the current total for future checks
        
        
        Debug.Log($"Collectible instantiated. Total now: {total}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected a collectible.");
            OnCollected?.Invoke();
            Destroy(gameObject);

            collectedCount++;
            Debug.Log($"Collected count: {collectedCount} / Total: {total}");
            if (collectedCount >= total)
            {
                Debug.Log("All collectibles collected.");
                OnAllCollected?.Invoke();
            }
        }
    }

    public static void ResetCollectibles()
    {
        Debug.Log("Resetting collectible counts. Before reset - Total: " + total + ", Collected: " + collectedCount);
        total = 0;
        collectedCount = 0;
    
        Debug.Log("After reset - Total: " + total + ", Collected: " + collectedCount);
    }

    public static void ResetInitialization()
    {
        Debug.Log("Resetting level initialization flag.");
        levelInitialized = false;
    }
}
