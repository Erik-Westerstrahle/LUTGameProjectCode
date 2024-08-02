using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Add this line for URP
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private int currentHealth;

    public Volume postProcessVolume; // Reference to the Volume component for URP
    public float damageEffectDuration = 0.5f; // Duration of the damage effect

    private Vignette vignette;

private void Start()
{
    currentHealth = maxHealth;
    UpdateHearts();

    // Get the Vignette effect from the Volume
    if (postProcessVolume.profile.TryGet(out Vignette vignetteEffect))
    {
        vignette = vignetteEffect;
        vignette.intensity.value = 0f; // Ensure it's invisible at start
        Debug.Log("Vignette effect found and initialized.");
    }
    else
    {
        Debug.LogError("Vignette effect not found in the Volume.");
    }
}

    public void TakeDamage(int amount)
    {
        Debug.Log("you have taken damage");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHearts();
       StartCoroutine(DamageEffect()); // Trigger the damage effect
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    private IEnumerator DamageEffect()
    {
        if (vignette == null)
        {
            Debug.LogError("Vignette effect is not set. Aborting DamageEffect coroutine.");
            yield break;
        }
        
        float elapsedTime = 0f;
        float maxIntensity = 0.5f; // Max intensity of the vignette

        Debug.Log("Starting DamageEffect coroutine.");

        // Fade in
        while (elapsedTime < damageEffectDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0f, maxIntensity, elapsedTime / (damageEffectDuration / 2f));
            Debug.Log("Vignette intensity (fade in): " + vignette.intensity.value);
            yield return null;
        }

        elapsedTime = 0f;

        // Fade out
        while (elapsedTime < damageEffectDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(maxIntensity, 0f, elapsedTime / (damageEffectDuration / 2f));
            Debug.Log("Vignette intensity (fade out): " + vignette.intensity.value);
            yield return null;
        }

        vignette.intensity.value = 0f;
        Debug.Log("DamageEffect coroutine finished.");


    }

                // Make sure to enable the Vignette only at runtime
    private void OnEnable()
    {
        if (vignette != null)
        {
            vignette.intensity.value = 0f; // Ensure it's invisible at start
        }
    }

    private void OnDisable()
    {
        if (vignette != null)
        {
            vignette.intensity.value = 0f; // Ensure it's invisible when not active
             Debug.Log("Vignette damage effect disabled");
        }
    }
}
