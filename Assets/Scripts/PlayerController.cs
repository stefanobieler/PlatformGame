using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;

    private PlayerInputAction playerInput;
    private InputAction move;
    private InputAction climb;

    private Rigidbody2D playerRigidBody;
    private CapsuleCollider2D playerCollider;
    private Animator anim;


    private void Awake(){
        playerInput = new PlayerInputAction();
    }

    private void SetupInput(){
        //get the jump input
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Jump.Enable();
        //Setup movement input
        move = playerInput.Player.Move;
        move.Enable();
        //set up climbing input
        climb = playerInput.Player.Climb;
    }

    private void FixedUpdate(){
        MovePlayer();
    }

    private void MovePlayer(){
        float dir = move.ReadValue<float>();
        Vector2 newVelocity = new Vector2(dir * movementSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = newVelocity;
        bool isRunning = Mathf.Abs(dir) >= Mathf.Epsilon ? true : false;
        anim.SetBool("isRunning", isRunning);
        if(isRunning) transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1.0f);
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if(!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;

        Vector2 currentVelocity = playerRigidBody.velocity;
        playerRigidBody.velocity += new Vector2(0.0f, jumpForce);
        
    }

    private void OnEnable(){
        SetupInput();
        playerRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }
}
