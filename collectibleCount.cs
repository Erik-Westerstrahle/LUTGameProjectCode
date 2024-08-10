//collectibleCount.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class collectibleCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    int count;

//      public Image[] crystalImages; // Array to hold references to the crystal UI images
 //   public Color coloredCrystal = Color.green; // The color you want the crystal to change to

    void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
  Debug.Log("CollectibleCount script initialized.");
    }

    void start() => UpdateCount();

/*     void OnEnable() => collectible_script.OnCollected += onCollectibleCollected;
    void OnDisable() => collectible_script.OnCollected -= onCollectibleCollected; */
    
    
    void OnEnable()
    {
        collectible_script.OnCollected += onCollectibleCollected;
        collectible_script.OnAllCollected += onAllCollectiblesCollected;
    } 

     void OnDisable()
    {
        collectible_script.OnCollected -= onCollectibleCollected;
        collectible_script.OnAllCollected -= onAllCollectiblesCollected;
    } 
    
    void onCollectibleCollected()
    {
          Debug.Log("Collectible collected event received");
      //  text.text = (++count).ToString();
      count++;
      UpdateCount();

/*              // Update crystal image color
        if (count - 1 < crystalImages.Length)
        {
            crystalImages[count - 1].color = coloredCrystal;
        } */
    }

        void onAllCollectiblesCollected()
    {
        Debug.Log("All collectibles collected. Loading next level.");
        LevelManager.Instance.LoadNextLevel();
    }

    void UpdateCount()
    {
        text.text = $"{count} / {collectible_script.total}";
    
    
    }
    

}