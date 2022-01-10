using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float jumpForce = 25.0f;
    [SerializeField] private float climbSpeed = 25.0f;

    private PlayerInputAction playerInput;
    private InputAction move;
    private InputAction climb;

    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerFeet;
    private Animator anim;
    private float startGravityScale;
    private const string ANIM_IS_CLIMBING = "isClimbing";
    private const string ANIM_IS_RUNNING = "isRunning";

    private void Awake()
    {
        playerInput = new PlayerInputAction();
    }

    private void SetupInput()
    {
        //get the jump input
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Jump.Enable();
        //Setup movement input
        move = playerInput.Player.Move;
        move.Enable();
        //set up climbing input
        climb = playerInput.Player.Climb;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        PlayerClimb();
    }

    private void PlayerClimb()
    {
        if (!climb.enabled) return;

        float dir = climb.ReadValue<float>();
        Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, dir * climbSpeed);
        playerRigidBody.velocity = climbVelocity;
    }

    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.gameObject.layer == LayerMask.NameToLayer("Climbing"))
    //     {
    //         playerRigidBody.gravityScale = 0;
    //         climb.Enable();
    //     }

    // }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Climbing"))
        {
            playerRigidBody.gravityScale = 0;
            climb.Enable();

            if (playerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;


            anim.SetBool(ANIM_IS_CLIMBING, true);
            anim.speed = Mathf.Clamp(Mathf.Abs(playerRigidBody.velocity.y), 0, 1.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Climbing"))
        {
            playerRigidBody.gravityScale = startGravityScale;
            anim.SetBool(ANIM_IS_CLIMBING, false);
            climb.Disable();
        }
    }


    private void MovePlayer()
    {
        //move player
        float dir = move.ReadValue<float>();
        Vector2 moveVelocity = new Vector2(dir * moveSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = moveVelocity;

        //animate
        bool isRunning = Mathf.Abs(dir) >= Mathf.Epsilon ? true : false;
        anim.SetBool(ANIM_IS_RUNNING, isRunning);
        if(!isRunning){
            anim.speed = 0.5f;
        }else{
            anim.speed = 2.0f;
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1.0f);
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (!playerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;

        Vector2 currentVelocity = playerRigidBody.velocity;
        playerRigidBody.velocity += new Vector2(0.0f, jumpForce);

    }

    private void OnEnable()
    {
        SetupInput();
        playerRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerFeet = GetComponent<BoxCollider2D>();
        startGravityScale = playerRigidBody.gravityScale;
    }
}
