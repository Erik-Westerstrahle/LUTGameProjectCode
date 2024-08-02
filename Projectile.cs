using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
      public float lifespan = 5f; 
    private Vector3 targetDirection;
    private float timeSinceSpawn;

    public void SetDirection(Vector3 direction)
    {
        targetDirection = direction.normalized;
    }

    private void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;

                // Increment the time since spawn
        timeSinceSpawn += Time.deltaTime;

        // Check if the projectile should be destroyed
        if (timeSinceSpawn >= lifespan)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
                // Check if the projectile collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealth component on the player
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Deal damage to the player
                playerHealth.TakeDamage(1); // Assume each projectile deals 1 damage
            }
        }
        // Handle collision logic here (e.g., damage the player)
        // For now, just destroy the projectile
        Destroy(gameObject);
    }
}
