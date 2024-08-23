//collectibleCount.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class collectibleCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    int count;

     public Image crystalPrefab; // Prefab for the crystal image
    public Transform crystalParent; // Parent object to hold the crystal UI images
    public Color coloredCrystal = Color.green; // The color you want the crystal to change to



      public float crystalSpacing = 50f; // Spacing between crystals along the x-axis

      // public bool crystalsVisible = true; // Boolean to control the visibility of the crystal sprites
         private List<Image> crystalImages = new List<Image>(); // List to store references to dynamically created crystals

    void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
        Debug.Log("CollectibleCount script initialized.");

         // Call GenerateCrystals after Awake to ensure total collectibles are calculated
        StartCoroutine(InitializeCrystals());
        
    }

        IEnumerator InitializeCrystals()
    {
        // Delay until the collectibles have been fully initialized in the scene
        yield return new WaitForEndOfFrame();

        Debug.Log($"Total collectibles: {collectible_script.total}");

        // Now generate the crystals
        GenerateCrystals();
        UpdateCount();
    }

    void Start() => UpdateCount();

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

   void GenerateCrystals()
{
    // Check if crystalPrefab is assigned
    if (crystalPrefab == null)
    {
        Debug.LogError("Crystal Prefab is not assigned in the Inspector!");
        return;
    }

    // Remove existing crystals if any
/*     foreach (Transform child in crystalParent)
    {
        Destroy(child.gameObject);
    }
    crystalImages.Clear();
 */
    // Ensure collectible_script.total is correct before generating crystals
    for (int i = 0; i < collectible_script.total; i++)
    {
        // Instantiate a new crystal
        Image newCrystal = Instantiate(crystalPrefab, crystalParent);
        newCrystal.color = Color.black; // Initially, the crystal color is black

        // Position the crystal along the positive x-axis
        RectTransform crystalRectTransform = newCrystal.GetComponent<RectTransform>();
        crystalRectTransform.anchoredPosition = new Vector2(i * crystalSpacing, crystalRectTransform.anchoredPosition.y);

        // Set the visibility of the crystal based on the crystalsVisible boolean
      //  newCrystal.gameObject.SetActive(crystalsVisible);

        // Add to the list of crystal images
        crystalImages.Add(newCrystal);
    }
}

    
    void onCollectibleCollected()
    {
          Debug.Log("Collectible collected event received");
      //  text.text = (++count).ToString();
      count++;
      UpdateCount();

              // Update crystal image color
        if (count - 1 < crystalImages.Count)
        {
            crystalImages[count - 1].color = coloredCrystal;
        } 
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