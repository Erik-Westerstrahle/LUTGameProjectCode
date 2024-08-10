    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LevelResetTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Something entered the trigger.");
            // Check if the player has collided with the reset plane
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered the trigger. Resetting level.");
                // Reload the current scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                 collectible_script.ResetCollectibles(); // Reset counts
            }
        }
    }
