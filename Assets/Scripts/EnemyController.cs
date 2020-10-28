using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform hitCastPosition; //to check if enemy hits sth
    [SerializeField] Transform castingRayPosition;  //to cast ray to follow player

    [SerializeField] float hitRange;
    [SerializeField] float castRange;

    [SerializeField] Transform player;

    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    bool isFacingRight;
    Vector3 baseScale;

    Animator animator;

    [SerializeField] bool isEnemyPatrol;

    bool isSearching;
    bool isNearPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseScale = transform.localScale;
        isFacingRight = true;
        isNearPlayer = false;
        isSearching = false;
    }

    void FixedUpdate()
    {
        if (IsHittingWall() || IsNearEdge())
        {
            StopChase();
            Flip();
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

        return Physics2D.Linecast(hitCastPosition.position, targetPosition, 1 << LayerMask.NameToLayer("Ground"));
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
}
