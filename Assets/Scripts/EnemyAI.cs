using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float attackRange = 1.5f;
    public int health = 3;

    public int damage = 10;
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackRange)
            {
                // Move towards the player
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else
            {
                if (Time.time >= nextAttackTime)
                {
                    AttackPlayer();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Enemy attacks!");

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
