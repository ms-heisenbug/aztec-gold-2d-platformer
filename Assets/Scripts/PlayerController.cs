using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private float jumpForce;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Collider2D collider;

    private PlayerInput controls;

    private bool facingRight;

    //[SerializeField] private LayerMask ground;

    private int maxHealth;
    private int currentHealth;
    [SerializeField] HealthManager healthManager;

    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    float nextAttack;

    private void Awake()
    {
        moveSpeed = 5.5f;
        jumpForce = 7;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();

        controls = new PlayerInput();

        facingRight = true;


        #region Health
        maxHealth = 100;
        healthManager.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        #endregion

        nextAttack = 0f;
    }

    private void Attack()
    {
        animator.Play("Player_Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, 1 << LayerMask.NameToLayer("Enemies"));

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(25);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    void Start()
    {
    }


    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(collider, 1 << LayerMask.NameToLayer("Ground"));

        if (Time.time >= nextAttack)
        {
            if (controls.Player.Attack.triggered)
            {
                Attack();
                nextAttack = Time.time  + 1f / attackRate;
            }
        }

        if (controls.Player.Jump.triggered && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.Play("Player_Jump");
        }

    }

    private void FixedUpdate()
    {
        float moveLeftRightInput = controls.Player.Movement.ReadValue<float>() * moveSpeed * Time.deltaTime;

        if ((moveLeftRightInput < 0 && facingRight) || (moveLeftRightInput > 0 && !facingRight))
        {
            Flip();
        }

        Vector3 currentPos = transform.position;
        currentPos.x += moveLeftRightInput;
        transform.position = currentPos;

        animator.SetFloat("Speed", Mathf.Abs(moveLeftRightInput));
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void TakeDamage(int dmg)
    {
        if (currentHealth - dmg <= 0)
        {
            animator.Play("Player_Death");
        }

        currentHealth -= dmg;
        healthManager.SetHealth(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Spike"))
        {
            TakeDamage(10);
        }
    }
}
