    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections; 

    public class LevelManager : MonoBehaviour
    {
         public static LevelManager Instance { get; private set; }

        private Animator animator;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

               // Get the Animator component from the Crossfade child object
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
        // Play the start animation when the level starts
        if (animator != null)
        {
            Debug.Log("Triggering Start animation.");
            animator.SetTrigger("Start");
        }
    }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the start animation when the new level is loaded
        if (animator != null)
        {
            animator.SetTrigger("Start");
        }
    }


        public void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
           //  transition.SetTrigger("Start")
            int nextSceneIndex = currentSceneIndex + 1;

            animator.SetTrigger("Start");

            

        Debug.Log($"Attempting to load next level: {nextSceneIndex}");

       

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Valid next level. Starting coroutine to load after animation.");
             animator.SetTrigger("Start");
            StartCoroutine(LoadSceneAfterAnimation(nextSceneIndex));
        }
        else
        {
            Debug.Log("No more levels to load. End of game.");
            // Handle end of game logic here
        }
        }

        public void RestartLevel()
        {
            collectible_script.ResetCollectibles();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }

        public void LoadLevel(string levelName)
        {
            collectible_script.ResetCollectibles();
            SceneManager.LoadScene(levelName);
            StartCoroutine(LoadSceneAfterAnimation(SceneManager.GetSceneByName(levelName).buildIndex));
            
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
