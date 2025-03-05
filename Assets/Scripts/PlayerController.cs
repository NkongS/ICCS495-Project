using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 5f;

    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask Enemy;

    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;

    private bool isMoving = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(Move(Vector3.forward));
            if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(Move(Vector3.back));
            if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(Move(Vector3.left));
            if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(Move(Vector3.right));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private System.Collections.IEnumerator Move(Vector3 direction)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + (direction * moveDistance);
        float elapsedTime = 0;

        while (elapsedTime < (moveDistance / moveSpeed))
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime * moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

    void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, Enemy);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>().TakeDamage(1);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player Died!");
        // Death animation/effects
        currentHealth = 0;
        healthBar.value = currentHealth;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        Invoke("RestartGame", 2f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            Die();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}