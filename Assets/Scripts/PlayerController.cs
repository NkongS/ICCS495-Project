using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 5f;
    private float moveCooldown = 0.35f;
    private float lastMoveTime = 0f;

    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask Enemy;

    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;

    private bool isMoving = false;

    private PlayerInputActions inputActions;

    public int score = 0;
    public TMP_Text scoreText;
    public GameObject gameOverMenu;
    public GameObject levelCompleteMenu;
    public GameObject[] characterModels;

    private bool canUseShield = true; // Tracks if the shield can be used
    private float shieldCooldown = 5f; // Cooldown duration for the shield
    private float shieldDuration = 2.5f; // Duration the shield stays active
    private GameObject activeShield; // Reference to the active shield

    private Vector3 moveDirection = Vector3.zero;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Attack.performed += OnAttack;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Disable();
    }

    void Start()
    {
        if (healthBar == null)
            healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        if (scoreText == null)
            scoreText = GameObject.Find("MainScoreText").GetComponent<TMP_Text>();

        if (gameOverMenu == null)
            gameOverMenu = GameObject.Find("GameOverMenu");

        if (levelCompleteMenu == null)
            levelCompleteMenu = GameObject.Find("LevelCompleteMenu");
            
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        UpdateScoreText();

        gameOverMenu.SetActive(false);
        levelCompleteMenu.SetActive(false);

        ActivateSelectedCharacter();
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        if (moveDirection != Vector3.zero && !isMoving && Time.time >= lastMoveTime + moveCooldown)
            {
                StartCoroutine(Move(moveDirection));
                lastMoveTime = Time.time;
            }
    }

    void ActivateSelectedCharacter()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0); // Default to Character 1

        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].SetActive(i == selectedCharacter);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y);

        if (moveDirection != Vector3.zero && !isMoving && Time.time >= lastMoveTime + moveCooldown)
        {
            StartCoroutine(Move(moveDirection));
            lastMoveTime = Time.time;
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveDirection = Vector3.zero;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Attack();
    }

    private System.Collections.IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = targetRotation;
        }

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
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);

        switch (selectedCharacter)
        {
            case 0: // Character 0: 180-degree attack in front
                Attack180Degree();
                break;

            case 1: // Character 1: Shoot bullets
                ShootBullet();
                break;

            case 2: // Character 2: 360-degree attack with shield
                Attack360WithShield();
                break;

            default: // Default to character 0's attack
                Debug.LogWarning("Unknown character selected!");
                Attack180Degree();
                break;
        }
    }

    void Attack180Degree()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, Enemy);

        foreach (Collider enemy in hitEnemies)
        {
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToEnemy);

            if (angle <= 90)
            {
                enemy.GetComponent<EnemyAI>().TakeDamage(1);
            }
        }
    }

    void ShootBullet()
    {
        GameObject bulletPrefab = Resources.Load<GameObject>("BulletPrefab");
        if (bulletPrefab == null)
        {
            Debug.LogError("BulletPrefab not found in Resources folder!");
            return;
        }

        // Offset the spawn position slightly forward and upward
        Vector3 spawnPosition = attackPoint.position + transform.forward * 0.5f + Vector3.up * 0.5f;

        // Instantiate the bullet and set its rotation to match the player's rotation
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * 20f; // Set bullet velocity in the player's forward direction
        }

        Destroy(bullet, (attackRange * 5f) / 20f); // Destroy bullet after it surpasses 5x attack range
    }

    void Attack360WithShield()
    {
        // 360-degree attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, Enemy);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>().TakeDamage(1);
        }

        if (!canUseShield) return; // Exit if shield is on cooldown

        // Spawn the shield slightly higher than the player
        if (activeShield == null)
        {
            Vector3 shieldPosition = transform.position + Vector3.up * 1.5f; // Adjust height as needed
            activeShield = Instantiate(Resources.Load<GameObject>("ShieldPrefab"), shieldPosition, Quaternion.identity);
            activeShield.transform.SetParent(transform); // Attach the shield to the player
        }

        // Start the shield duration and cooldown timers
        StartCoroutine(HandleShieldLifetime());
        StartCoroutine(HandleShieldCooldown());
    }

    private System.Collections.IEnumerator HandleShieldLifetime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shieldDuration)
        {
            if (activeShield == null) yield break; // Exit if the shield is destroyed early
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the shield after 7 seconds
        if (activeShield != null)
        {
            Destroy(activeShield);
        }
    }

    private System.Collections.IEnumerator HandleShieldCooldown()
    {
        canUseShield = false; // Disable shield usage
        yield return new WaitForSeconds(shieldCooldown); // Wait for cooldown duration
        canUseShield = true; // Re-enable shield usage
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

        inputActions.Player.Disable();

        gameOverMenu.SetActive(true);
        gameOverMenu.transform.Find("ScoreText").GetComponent<TMP_Text>().text = "Score: " + score;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            Die();
        }
        else if (other.CompareTag("End"))
        {
            LevelComplete();
            Debug.Log("Level Complete!");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelSelecter()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("LevelSelectionScene");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    void LevelComplete()
    {
        Time.timeScale = 0f;

        inputActions.Player.Disable();

        levelCompleteMenu.SetActive(true);
        levelCompleteMenu.transform.Find("ScoreText").GetComponent<TMP_Text>().text = "Score: " + score;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}