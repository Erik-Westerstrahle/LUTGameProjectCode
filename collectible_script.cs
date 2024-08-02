using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class collectible_script : MonoBehaviour
{
    // Start is called before the first frame update
/*     void Start()
    {
         ResetCollectibles();
    } */
    public static event Action OnCollected;
         public static event Action OnAllCollected;

         public static int total;
         private static int collectedCount;

    

   // void Awake() => total++; 

   void Awake()
    {
        total++;
    }

    // Update is called once per frame
    void Update()
    {
      //  transform.localRotation = Quaternion.Euler(90f, Time.time * 100f, 0);
    }

/*     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected?.Invoke();
            Destroy(gameObject);
        }
    } */
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected?.Invoke();
            Destroy(gameObject);

            collectedCount++;
            if (collectedCount >= total)
            {
                OnAllCollected?.Invoke();
            }
        }
    }

        public static void ResetCollectibles()
    {
         Debug.Log("Resetting collectible counts");
        total = 0;
        collectedCount = 0;
    }
}
