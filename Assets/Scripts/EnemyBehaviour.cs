using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform player;

    float rangeOfSight;
    float moveSpeed;
    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rangeOfSight = 3.5f;
        animator = GetComponent<Animator>();
        moveSpeed = 1f;
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if(distanceToPlayer < rangeOfSight)
        {
            Chase();
        }
        else
        {
            StopChase();
        }
    }

    private void Chase()
    {
        Debug.Log("Chase");

        animator.SetBool("IsMoving", true);

        if(transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
        }

    }

    private void StopChase()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }
}
