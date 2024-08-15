using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRadius = 10.0f;
    public float approachSpeed = 1.0f;
    public string playerTag = "Player"; // Make sure your player is tagged as "Player"

    private Transform player;
    private Vector3 randomDirection;
    private float changeDirectionTime = 2.0f;
    private float timer;

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
                // Move towards the player
                movementDirection = (player.position - transform.position).normalized;
                transform.position += movementDirection * approachSpeed * Time.deltaTime;
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

                movementDirection = randomDirection;
                transform.position += randomDirection * speed * Time.deltaTime;
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

            movementDirection = randomDirection;
            transform.position += randomDirection * speed * Time.deltaTime;
        }

        // Rotate the enemy to face the movement direction
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
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
