using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRadius = 10.0f;
    public float approachSpeed = 1.0f;
    public float chargeSpeed = 5.0f; // Speed during the charge
    public float chargeDuration = 1.0f; // Duration of the charge
    public float chargeCooldown = 2.0f; // Cooldown before the next charge
    public string playerTag = "Player"; // Make sure your player is tagged as "Player"

        public GameObject chargeMesh; // Reference to the mesh that will become visible during the charge

    public AudioSource audioSource;
     public AudioClip chargeSound;

    private Transform player;
    private Vector3 randomDirection;

     private Vector3 chargeDirection; // Direction the enemy charges (add this)
    private float changeDirectionTime = 2.0f;
    private float timer;

    private bool isCharging = false;
    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;
    private bool playerInRadius = false;  // New flag to track if the player is in radius

    private void Start()
    {
        timer = changeDirectionTime;
        randomDirection = GetRandomDirection();

        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

                // Ensure the charge mesh is initially invisible
        if (chargeMesh != null)
        {
            chargeMesh.SetActive(false);
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

        Vector3 movementDirection = Vector3.zero;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius)
            {
                if (!playerInRadius || cooldownTimer <= 0f)  // Trigger charge when player enters or cooldown is over
                {
                    playerInRadius = true;
                    StartCharge();
                }

                if (isCharging)
                {
                    ChargeTowardsPlayer();
                }
                else
                {
                    // Move towards the player normally if not charging
                    movementDirection = (player.position - transform.position).normalized;
                    transform.position += movementDirection * approachSpeed * Time.deltaTime;
                }
            }
            else
            {
                // Player left the detection radius
                playerInRadius = false;

                // Random movement
                cooldownTimer = Mathf.Max(cooldownTimer - Time.deltaTime, 0f);
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    randomDirection = GetRandomDirection();
                    timer = changeDirectionTime;
                }

                movementDirection = randomDirection;
                transform.position += randomDirection * speed * Time.deltaTime;
            }
        }
        else
        {
            // Random movement if player is not found
            cooldownTimer = Mathf.Max(cooldownTimer - Time.deltaTime, 0f);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                randomDirection = GetRandomDirection();
                timer = changeDirectionTime;
            }

            movementDirection = randomDirection;
            transform.position += randomDirection * speed * Time.deltaTime;
        }

        // Rotate the enemy to face the movement direction
        if (movementDirection != Vector3.zero && !isCharging)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    private void StartCharge()
    {
        if (cooldownTimer <= 0f)
        {
            // Start charging towards the player
            isCharging = true;
            chargeTimer = chargeDuration;
            cooldownTimer = chargeCooldown;  // Start cooldown immediately after charge starts

             // Calculate and store the charge direction
            chargeDirection = (player.position - transform.position).normalized;

                // Make the charge mesh visible
            if (chargeMesh != null)
            {
                chargeMesh.SetActive(true);
            }
                        if (audioSource != null && chargeSound != null)
            {
                audioSource.PlayOneShot(chargeSound);
            }
        }
    }

    private void ChargeTowardsPlayer()
    {
        if (player == null) return;

        chargeTimer -= Time.deltaTime;

        // Move towards the player with charge speed
     //   Vector3 chargeDirection = (player.position - transform.position).normalized;
      //  transform.position += chargeDirection * chargeSpeed * Time.deltaTime;
         transform.position += chargeDirection * chargeSpeed * Time.deltaTime;

        // Rotate to face the player
        Quaternion targetRotation = Quaternion.LookRotation(chargeDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * chargeSpeed);

        if (chargeTimer <= 0f)
        {
            isCharging = false;

                        // Make the charge mesh invisible
            if (chargeMesh != null)
            {
                chargeMesh.SetActive(false);
            }
        }
    }

    private Vector3 GetRandomDirection()
    {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
