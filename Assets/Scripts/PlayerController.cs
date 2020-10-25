using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

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

    [SerializeField] private LayerMask ground;

    private void Awake()
    {
        moveSpeed = 5.5f;
        jumpForce = 7;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();

        controls = new PlayerInput();

        facingRight = true;

    }

    private void Attack()
    {

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
        isGrounded = Physics2D.IsTouchingLayers(collider, ground);

        if (controls.Player.Attack.triggered)
        {
            animator.Play("Player_Attack");
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
}
