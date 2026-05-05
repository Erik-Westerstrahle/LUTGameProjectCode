using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRadius = 10.0f; // Aggro radius
    public float attackRadius = 2.0f; // Attack radius
    public float attackCooldown = 1.5f; // Time between attacks
    public string playerTag = "Player"; // Tag for identifying the player

    public Animator animator; // Reference to the Animator component
    public string walkAnimation = "groundEnemyWalkAnim"; // Name of the walk animation
    public string attackAnimation = "groundEnemyAttackAnim"; // Name of the attack animation

    public BoxCollider attackCollider; // Collider used for detecting attacks
    public int damage = 1; // Damage to inlict on the player

    public AudioSource attackAudioSource; // Audio source for playing atack sound
    public AudioClip attackAudioClip; // Attack sound effect

    private Transform player;
    private float attackTimer = 0f;
    private bool isAttacking = false;

        private bool playerInAttackRange = false;

    private void Start()
    {
        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Ensure the attack collider is a trigger
        if (attackCollider != null)
        {
            attackCollider.isTrigger = true;
        }

        // Play the walk animation by default
        if (animator != null && !string.IsNullOrEmpty(walkAnimation))
        {
            animator.Play(walkAnimation);
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

            if (distanceToPlayer <= attackRadius)
            {
                if (!isAttacking && attackTimer <= 0f)
                {
                    // Player is within attack range, start attack
                    StartCoroutine(AttackPlayer());
                }
            }
            else if (distanceToPlayer <= detectionRadius)
            {
                // Player is within detection range but outside attack range, move towards the player
                MoveTowardsPlayer();
            }

            // Decrease the attack cooldown timer
            if (attackTimer > 0f)
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (animator != null && !isAttacking && !string.IsNullOrEmpty(walkAnimation))
        {
            animator.Play(walkAnimation);
        }

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate the enemy to face the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        if (animator != null && !string.IsNullOrEmpty(attackAnimation))
        {
            animator.Play(attackAnimation);
        }

            // Play the attack sound
        if (attackAudioSource != null && attackAudioClip != null)
        {
            attackAudioSource.PlayOneShot(attackAudioClip);
        }


        // Wait for the attack animation to complete
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // After the attack, reset cooldown and return to moving
        attackTimer = attackCooldown;
        isAttacking = false;

        if (animator != null && !string.IsNullOrEmpty(walkAnimation))
        {
            animator.Play(walkAnimation);
        }
    }



        private void OnTriggerStay(Collider other)
    {
        if (isAttacking && other.CompareTag(playerTag))
        {
            Debug.Log("Player in attack collider. Inflicting damage.");
            // Inflict damage on the player when within attack collider
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
