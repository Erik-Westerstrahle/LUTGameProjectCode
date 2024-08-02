using System.Collections;
using UnityEngine;

public class EnemyControllerProjectileController : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRadius = 10.0f;
    public float approachSpeed = 1.0f;
    public string playerTag = "Player"; // Make sure your player is tagged as "Player"
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Point from where the projectile will be fired
    public float shootingInterval = 1.0f; // Time interval between shots

    //    public Vector3 roamingAreaMin; // Minimum coordinates of the roaming area
  //  public Vector3 roamingAreaMax; // Maximum coordinates of the roaming area

   // public Vector3 roamingAreaCenter; // Center of the roaming area
  //  public Vector3 roamingAreaExtents; // Extents of the roaming area (half-size in each direction)
   
   public BoxCollider roamingArea; // Reference to the BoxCollider defining the roaming area

    private Transform player;
    private Vector3 randomDirection;
    private float changeDirectionTime = 2.0f;
    private float timer;
    private float shootingTimer;

    private void Start()
    {
        timer = changeDirectionTime;
        randomDirection = GetRandomDirection();
        shootingTimer = shootingInterval;

        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            // Try to find the player again if it hasn't been found yet
            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius)
            {
                // Move towards the player
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position = ClampPositionToRoamingArea(transform.position + direction * approachSpeed * Time.deltaTime);

                // Shoot at the player
                shootingTimer -= Time.deltaTime;
                if (shootingTimer <= 0)
                {
                    Shoot(player.position);
                    shootingTimer = shootingInterval;
                }
            }
            else
            {
                // Random movement
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    randomDirection = GetRandomDirection();
                    timer = changeDirectionTime;
                }

                Vector3 newPosition = transform.position + randomDirection * speed * Time.deltaTime;
                transform.position = ClampPositionToRoamingArea(newPosition);
            }
        }
        else
        {
            // Random movement if player is not found
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                randomDirection = GetRandomDirection();
                timer = changeDirectionTime;
            }

            Vector3 newPosition = transform.position + randomDirection * speed * Time.deltaTime;
            transform.position = ClampPositionToRoamingArea(newPosition);
        }
    }

    private void Shoot(Vector3 playerPosition)
    {
        // Instantiate the projectile at the fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        // Calculate direction to player
        Vector3 direction = (playerPosition - firePoint.position).normalized;

        // Set the direction for the projectile
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
        }
    }

    private Vector3 GetRandomDirection()
    {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
    }

    private Vector3 ClampPositionToRoamingArea(Vector3 position)
    {
        if (roamingArea != null)
        {
            Vector3 min = roamingArea.bounds.min;
            Vector3 max = roamingArea.bounds.max;

            return new Vector3(
                Mathf.Clamp(position.x, min.x, max.x),
                Mathf.Clamp(position.y, min.y, max.y),
                Mathf.Clamp(position.z, min.z, max.z)
            );
        }
        return position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (roamingArea != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(roamingArea.bounds.center, roamingArea.bounds.size);
        }
    }
}
