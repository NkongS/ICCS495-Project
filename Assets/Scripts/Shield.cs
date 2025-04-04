using UnityEngine;

public class Shield : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(3); // Deal damage to the enemy
            }

            Destroy(gameObject); // Destroy the shield when hit by an enemy
        }
    }
}