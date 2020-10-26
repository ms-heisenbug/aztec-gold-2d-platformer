using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed = 2f;

    [SerializeField] Transform castPos;
    float baseCastDistance = 0.3f;
    bool facingRight;
    Vector3 baseScale;

    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        baseScale = transform.localScale;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float velocityX = moveSpeed;

        if (!facingRight)
        {
            velocityX = -moveSpeed;
        }

        animator.SetFloat("Speed", Mathf.Abs(velocityX));
        rb.velocity = new Vector2(velocityX, 0);

        if (IsHittingWall() || IsNearEdge())
        {
            Flip();
        }
    }

    bool IsHittingWall()
    {
        float castDistance = baseCastDistance;

        if (!facingRight)
        {
            castDistance = -baseCastDistance;
        }

        Vector3 targetPos = castPos.position;
        targetPos.x += castDistance;

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        return Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"));
    }

    bool IsNearEdge()
    {
        float castDistance = baseCastDistance;

        Vector3 targetPos = castPos.position;
        targetPos.y -= castDistance;

        Debug.DrawLine(castPos.position, targetPos, Color.yellow);

        return !Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"));
    }

    void Flip()
    {
        Vector3 newScale = baseScale;

        if (facingRight)
        {
            newScale.x = -baseScale.x;
        }

        transform.localScale = newScale;

        facingRight = !facingRight;
    }
}
