using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform player;

    float rangeOfSight;
    float moveSpeed;
    Rigidbody2D rb;
    Animator animator;
    bool isFacingRight;

    [SerializeField] Transform castPoint;
    private bool isAgro;
    private bool isSearching;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rangeOfSight = 3.5f;
        animator = GetComponent<Animator>();
        moveSpeed = 2f;
        isFacingRight = true;
        isAgro = false;
    }

    void FixedUpdate()
    {
        if (CanSeePlayer(rangeOfSight))
        {
            isAgro = true;
        }
        else
        {
            if (isAgro)
            {
                if (!isSearching)
                {
                    isSearching = true;
                    Invoke("StopChase", 3);
                }
            }
        }

        if (isAgro)
        {
            Chase();
        }

        animator.SetFloat("Speed", rb.velocity.x);
    }

    private void Chase()
    {
        Debug.Log("chase");
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
        isAgro = false;
        isSearching = false;
        rb.velocity = Vector2.zero;
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        if (!isFacingRight)
        {
            castDist = -distance;
        }

        Vector2 endPosition = castPoint.position + Vector3.right * castDist;
        RaycastHit2D hit2D = Physics2D.Linecast(castPoint.position, endPosition, 1 << LayerMask.NameToLayer("Playground"));

        if (hit2D.collider != null)
        {
            val = hit2D.collider.gameObject.CompareTag("Player");
            Debug.DrawLine(castPoint.position, hit2D.point, Color.red);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPosition, Color.green);
        }


        return val;
    }
}
