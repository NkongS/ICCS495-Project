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

    private PlayerController playerController;
    private bool isDead = false;

    void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (player != null && !isDead)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackRange)
            {
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
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        playerController.AddScore(100);

        Destroy(gameObject);
    }
}