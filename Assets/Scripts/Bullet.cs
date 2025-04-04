using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f; // Speed of the bullet
    public float maxDistance = 50f; // Maximum distance the bullet can travel
    public int damage = 1; // Damage dealt by the bullet

    private Vector3 startPosition;
    private Rigidbody rb;

    void Start()
    {
        // Record the starting position of the bullet
        startPosition = transform.position;

        // Get the Rigidbody component and set its velocity
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed; // Use the forward direction of the bullet's transform
        }
    }

    void Update()
    {
        // Destroy the bullet if it exceeds its maximum distance
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hits an enemy
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Deal damage to the enemy
            }

            // Destroy the bullet after hitting an enemy
            Destroy(gameObject);
        }
    }
}