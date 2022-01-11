using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : BaseState
{

    private PlayerStateMachine playerMachine;
    private InputAction moveInput;
    private InputAction climbInput;
    private Rigidbody2D playerRigidBody;
    private float runAnimSpeed = 1.5f;
    private float moveSpeed;
    private float moveDir;
    private bool canClimb;

    public PlayerRunState(BaseStateMachine stateMachine) : base(stateMachine)
    {
        playerMachine = stateMachine as PlayerStateMachine;
        playerRigidBody = playerMachine.playerRigidBody;
        moveInput = playerMachine.playerInput.Player.Move;
        climbInput = playerMachine.playerInput.Player.Climb;
        moveSpeed = playerMachine.moveSpeed;
        canClimb = false;
    }

    public override void Start()
    {
        base.Start();
        playerMachine.anim.speed = runAnimSpeed;
        moveInput.Enable();
        playerMachine.playerInput.Player.Jump.performed += PlayerJump;
        playerMachine.playerInput.Player.Jump.Enable();
        playerMachine.PlayerNearClimbArea += OnPlayerNearClimbArea;
    }
    public override void Update()
    {
        base.Update();
        moveDir = moveInput.ReadValue<float>();

        bool isMoving = Mathf.Abs(moveDir) > Mathf.Epsilon ? true : false;

        playerMachine.anim.SetBool("isRunning", isMoving);

        if (!isMoving)
        {
            ChangeToIdleState();
        }
        else if (canClimb)
        {
            float climbDir = climbInput.ReadValue<float>();
            bool isClimbing = climbDir > Mathf.Epsilon ? true : false;
            if (isClimbing) ChangeToClimbState();
        }

        if (isMoving)
        {
            playerMachine.transform.localScale = new Vector2(Mathf.Sign(moveDir), 1.0f);
        }
    }



    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float currentYVelocity = playerRigidBody.velocity.y;
        Vector2 newVelocity = new Vector2(moveDir * moveSpeed, currentYVelocity);
        playerRigidBody.velocity = newVelocity;
    }

    public override void Exit()
    {
        base.Exit();
        moveInput.Disable();
        playerMachine.playerInput.Player.Jump.Disable();
        playerMachine.playerInput.Player.Jump.performed -= PlayerJump;
        playerMachine.PlayerNearClimbArea -= OnPlayerNearClimbArea;
        playerRigidBody.velocity = Vector2.zero;
        playerMachine.anim.SetBool("isRunning", false);
    }


    private void OnPlayerNearClimbArea(bool canClimb)
    {
        this.canClimb = canClimb;
        if (canClimb)
        {
            climbInput.Enable();
        }
        else
        {
            climbInput.Disable();
        }
    }

    private void PlayerJump(InputAction.CallbackContext obj)
    {
        previousState = this;
        nextState = playerMachine.states[(int)PlayerStateMachine.PlayerState.Jump];
        playerMachine.ChangeState(nextState);
    }
    private void ChangeToIdleState()
    {
        previousState = this;
        nextState = playerMachine.states[(int)PlayerStateMachine.PlayerState.Idle];
        playerMachine.ChangeState(nextState);

    }

    private void ChangeToClimbState()
    {
        previousState = this;
        nextState = playerMachine.states[(int)PlayerStateMachine.PlayerState.Climb];
        playerMachine.ChangeState(nextState);
    }


}
