using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private PlayerInputActions inputActions;

    public int score = 0;
    public TMP_Text scoreText;
    public GameObject gameOverMenu;
    public GameObject levelCompleteMenu;
    public GameObject[] characterModels;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Attack.performed += OnAttack;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Disable();
    }

    void Start()
    {
        if (healthBar == null)
            healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();

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
        if (!isMoving)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 direction = new Vector3(input.x, 0, input.y);

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = targetRotation;
            }

            StartCoroutine(Move(direction));
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Attack();
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