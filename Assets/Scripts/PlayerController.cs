using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 8.5f;
    public float dashSpeed = 13f;
    public float dashTime = 0.25f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundRadius = 0.25f;

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip hitSound;

    [Header("Input Asset (Optional)")]
    public InputActionAsset inputAsset;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private GameManager gm;

    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction pauseAction;

    private bool isGrounded;
    private bool isDashing;
    private float dashTimer;
    private int lives = 3;
    private int score = 0;
    private float scoreTimer = 0f;
    private bool isInvincible = false;
    private bool isDead = false;

    public bool IsDead => isDead;
    public int Lives => lives;
    public int Score => score;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) { Destroy(gameObject); return; }

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gm = Object.FindAnyObjectByType<GameManager>();

        if (groundCheck == null)
        {
            groundCheck = transform.Find("GroundCheck");
            if (groundCheck == null)
            {
                var gc = new GameObject("GroundCheck");
                gc.transform.SetParent(transform);
                gc.transform.localPosition = new Vector3(0, -0.6f, 0);
                groundCheck = gc.transform;
            }
        }

        SetupInputs();
    }

    private void SetupInputs()
    {
        if (inputAsset == null)
        {
            inputAsset = Resources.Load<InputActionAsset>("PlayerControls");
        }

        if (inputAsset != null)
        {
            jumpAction = inputAsset.FindAction("Jump");
            dashAction = inputAsset.FindAction("Dash");
            pauseAction = inputAsset.FindAction("Pause");

            jumpAction?.Enable();
            dashAction?.Enable();
            pauseAction?.Enable();
        }
    }

    private void OnDisable()
    {
        jumpAction?.Disable();
        dashAction?.Disable();
        pauseAction?.Disable();
    }

    private void Update()
    {
        if (isDead) return;

        // Ground check
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        }

        // Score increases over distance/time
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 0.1f)
        {
            scoreTimer = 0f;
            AddScore(1);
        }

        // Handle Input
        bool jumpPressed = (jumpAction != null && jumpAction.triggered) ||
                           (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame);

        bool dashPressed = (dashAction != null && dashAction.triggered) ||
                           (Keyboard.current != null && (Keyboard.current.leftShiftKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame));

        bool pausePressed = (pauseAction != null && pauseAction.triggered) ||
                            (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame);

        if (pausePressed && gm != null)
        {
            gm.TogglePause();
        }

        if (jumpPressed && isGrounded && !isDashing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if (jumpSound != null) AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            PlayAnimation("Jump");
        }

        if (dashPressed && !isDashing)
        {
            isDashing = true;
            dashTimer = dashTime;
            if (dashSound != null) AudioSource.PlayClipAtPoint(dashSound, transform.position);
            PlayAnimation("Dash");
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }

        // Update movement animation if not jumping or dashing or hit
        if (!isDashing)
        {
            if (isGrounded)
            {
                PlayAnimation("Run");
            }
            else
            {
                PlayAnimation("Jump");
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        var interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.Interact(this);
        }
    }

    public void TakeDamage()
    {
        if (isDead || isInvincible) return;

        lives--;
        if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);

        if (gm != null)
        {
            gm.UpdateLives(lives);
        }

        if (lives <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashAndInvincible());
            PlayAnimation("Hit");
        }
    }

    private void Die()
    {
        isDead = true;
        PlayAnimation("Death");
        if (gm != null)
        {
            gm.GameOver();
        }
    }

    public void AddScore(int value)
    {
        if (isDead) return;
        score += value;
        if (gm != null)
        {
            gm.UpdateScore(score);
        }
    }

    private void PlayAnimation(string stateName)
    {
        if (anim != null && anim.hasBoundPlayables)
        {
            anim.Play(stateName);
        }
    }

    private IEnumerator FlashAndInvincible()
    {
        isInvincible = true;
        if (sr != null)
        {
            Color orig = sr.color;
            for (int i = 0; i < 4; i++)
            {
                sr.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                sr.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
            sr.color = orig;
        }
        else
        {
            yield return new WaitForSeconds(0.8f);
        }
        isInvincible = false;
    }
}
