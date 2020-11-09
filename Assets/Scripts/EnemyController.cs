using System;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform hitCastPosition; //to check if enemy hits sth
    [SerializeField] Transform castingRayPosition;  //to cast ray to follow player
    [SerializeField] Transform attackPoint;  

    [SerializeField] float hitRange;
    [SerializeField] float castRange;

    [SerializeField] Transform player;
    [SerializeField] GameObject enemyObj;

    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    bool isFacingRight;
    Vector3 baseScale;

    Animator animator;

    [SerializeField] bool isEnemyPatrol;

    bool isSearching;
    bool isNearPlayer;

    int health;
    int currentHealth;

    bool isAttacking;
    float nextAttack;
    float attackRate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseScale = transform.localScale;
        isNearPlayer = false;
        isSearching = false;
        health = 100;
        currentHealth = health;
        isAttacking = false;
        nextAttack = 0f;
        attackRate = 0.5f;
        isFacingRight = true;
    }

    void FixedUpdate()
    {
        if (IsHittingWall() || IsNearEdge())
        {
            StopChase();
            Flip();
        }
        else if (IsPlayerWithinAttackRange() && Time.time >= nextAttack)
        {
            rb.velocity = Vector2.zero;
            animator.Play("Enemy_Attack");
            Attack();
            nextAttack = Time.time + 1f / attackRate;
        }
        else if (IsPlayerTooClose())
        {
            StopChase();
        }
        else
        {
            StandBy();
        }

        if (isEnemyPatrol)
        {
            float velocityX = GetValueBasedOnFacingDirection(moveSpeed);
            animator.SetFloat("Speed", moveSpeed);
            rb.velocity = new Vector2(velocityX, 0);
        }

        isAttacking = false;
    }

    private bool IsPlayerTooClose()
    {
        return Physics2D.OverlapCircleAll(castingRayPosition.position, 0.2f, 1 << LayerMask.NameToLayer("Playground")).Any();
    }

    private void Attack()
    {
        if(!isAttacking)
        {
            player.GetComponent<PlayerController>().TakeDamage(5);
            isAttacking = true;
        }
    }

    private bool IsPlayerWithinAttackRange()
    {
        return Physics2D.OverlapCircleAll(attackPoint.position, 2f, 1 << LayerMask.NameToLayer("Player")).Any();
    }

    private void Flip()
    {
        Vector3 newScale = baseScale;

        if (isFacingRight)
        {
            newScale.x = -baseScale.x;
        }

        transform.localScale = newScale;

        isFacingRight = !isFacingRight;
    }

    private bool IsNearEdge()
    {
        float castDist = hitRange;

        Vector3 targetPosition = hitCastPosition.position;
        targetPosition.y -= hitRange;

        Debug.DrawLine(hitCastPosition.position, targetPosition, Color.yellow);

        return !Physics2D.Linecast(hitCastPosition.position, targetPosition, 1 << LayerMask.NameToLayer("Ground"));
    }

    private float GetValueBasedOnFacingDirection(float dist)
    {
        if (!isFacingRight)
        {
            return -dist;
        }
        return dist;
    }

    private bool IsHittingWall()
    {
        float castDist = GetValueBasedOnFacingDirection(hitRange);

        Vector3 targetPosition = hitCastPosition.position;
        targetPosition.x += castDist;

        Debug.DrawLine(hitCastPosition.position, targetPosition, Color.red);

        bool isHittingWall = Physics2D.Linecast(hitCastPosition.position, targetPosition, 1 << LayerMask.NameToLayer("Ground"));
        bool isHittingSpikes = Physics2D.Linecast(hitCastPosition.position, targetPosition, 1 << LayerMask.NameToLayer("Obstacles"));

        return isHittingWall || isHittingSpikes;
    }

    private void StandBy()
    {
        if (CanSeePlayer(castRange))
        {
            isNearPlayer = true;
        }
        else
        {
            if (isNearPlayer)
            {
                if (!isSearching)
                {
                    isSearching = true;
                    Invoke("StopChase", 3);
                }
            }
        }

        if (isNearPlayer)
        {
            Chase();
        }

        animator.SetFloat("Speed", rb.velocity.x);
    }

    private void Chase()
    {
        if (transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
            isFacingRight = true;
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
            isFacingRight = false;
        }
    }

    private void StopChase()
    {
        isNearPlayer = false;
        isSearching = false;

        if (!isEnemyPatrol)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            float velocityX = GetValueBasedOnFacingDirection(moveSpeed);
            animator.SetFloat("Speed", velocityX);
            rb.velocity = new Vector2(velocityX, 0);
        }
    }

    private bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = GetValueBasedOnFacingDirection(distance);

        Vector2 endPosition = castingRayPosition.position + Vector3.right * castDist;
        RaycastHit2D hit2D = Physics2D.Linecast(castingRayPosition.position, endPosition, 1 << LayerMask.NameToLayer("Playground"));

        if(hit2D.collider != null)
        {
            val = hit2D.collider.gameObject.CompareTag("Player");
            Debug.DrawLine(castingRayPosition.position, hit2D.point, Color.red);
        }
        else
        {
            Debug.DrawLine(castingRayPosition.position, endPosition, Color.green);
        }

        return val;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        GameObject.Destroy(enemyObj, 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Lava"))
        {
            Die();
        }
    }
}
