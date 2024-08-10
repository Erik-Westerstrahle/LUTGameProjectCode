using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private Animator animator;

    private void Awake()
    {
        Debug.Log("LevelManager Awake called.");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("LevelManager instance created.");
        }
        else
        {
            Debug.Log("LevelManager instance already exists. Destroying duplicate.");
            Destroy(gameObject);
        }

        animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            Debug.Log("Animator component found.");
        }
        else
        {
            Debug.LogError("Animator component not found!");
        }
    }

    private void Start()
    {
        if (animator != null)
        {
            Debug.Log("Triggering Start animation.");
            animator.SetTrigger("Start");
        }
    }

    public void LoadNextLevel()
    {
        Debug.Log("Loading next level.");

        collectible_script.ResetInitialization(); // Reset initialization flag
        collectible_script.ResetCollectibles(); // Reset counts

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        animator.SetTrigger("Start");

        Debug.Log($"Attempting to load next level: {nextSceneIndex}");

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Valid next level. Starting coroutine to load after animation.");
            StartCoroutine(LoadSceneAfterAnimation(nextSceneIndex));
        }
        else
        {
            Debug.Log("No more levels to load. End of game.");
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting level.");

        collectible_script.ResetInitialization(); // Ensure collectible counts are reset on level restart
        collectible_script.ResetCollectibles();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string levelName)
    {
        Debug.Log($"Loading level: {levelName}");

        collectible_script.ResetInitialization(); // Reset initialization flag
        
        SceneManager.LoadScene(levelName);
        StartCoroutine(LoadSceneAfterAnimation(SceneManager.GetSceneByName(levelName).buildIndex));
        collectible_script.ResetCollectibles(); // Reset counts
    }

    private IEnumerator LoadSceneAfterAnimation(int sceneIndex)
    {
        Debug.Log("Starting LoadSceneAfterAnimation coroutine.");

        if (animator != null)
        {
            Debug.Log("Waiting for end animation to finish.");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.LogError("Animator is null. Skipping animation wait.");
        }

        Debug.Log($"Loading scene index: {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
    }
}
