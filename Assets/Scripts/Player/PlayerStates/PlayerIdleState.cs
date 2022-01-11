using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : BaseState
{

    PlayerStateMachine playerMachine;
    InputAction moveInput;
    InputAction climbInput;
    Rigidbody2D playerRigidBody;

    float idleAnimationSpeed = 0.5f;
    float moveDir;
    bool canClimb;

    public PlayerIdleState(BaseStateMachine stateMachine) : base(stateMachine)
    {
        playerMachine = stateMachine as PlayerStateMachine;
        playerRigidBody = playerMachine.playerRigidBody;
        moveInput = playerMachine.playerInput.Player.Move;
        climbInput = playerMachine.playerInput.Player.Climb;
    }

    public override void Start()
    {
        base.Start();
        playerMachine.anim.speed = idleAnimationSpeed;
        moveInput.Enable();
        playerMachine.playerInput.Player.Jump.performed += PlayerJump;
        playerMachine.playerInput.Player.Jump.Enable();
        playerMachine.PlayerNearClimbArea += OnPlayerNearClimbArea;
    }

    private void PlayerJump(InputAction.CallbackContext obj)
    {
        previousState = this;
        nextState = playerMachine.states[(int)PlayerStateMachine.PlayerState.Jump];
        playerMachine.ChangeState(nextState);
    }

    public override void Update()
    {
        base.Update();
        moveDir = moveInput.ReadValue<float>();
        bool isMoving = Mathf.Abs(moveDir) > Mathf.Epsilon ? true : false;

        if (isMoving)
        {
            ChangeToRunState();
        }
        else if (canClimb)
        {
            float climbDir = climbInput.ReadValue<float>();
            bool isClimbing = climbDir > Mathf.Epsilon ? true : false;
            if(isClimbing) ChangeToClimbState();
        }
    }

    private void OnPlayerNearClimbArea(bool canClimb){
        this.canClimb = canClimb;

        if(canClimb){
            climbInput.Enable();
        }else{
            climbInput.Disable();
        }
    }

    private void ChangeToRunState()
    {

        previousState = this;
        nextState = playerMachine.states[(int)PlayerStateMachine.PlayerState.Run];
        playerMachine.ChangeState(nextState);
    }

    private void ChangeToClimbState(){
        previousState = this;
        nextState = playerMachine.states[(int)PlayerStateMachine.PlayerState.Climb];
        playerMachine.ChangeState(nextState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float currentYVelocity = playerRigidBody.velocity.y;
        playerRigidBody.velocity = new Vector2(moveDir, currentYVelocity);
    }

    public override void Exit()
    {
        moveInput.Disable();
        climbInput.Disable();
        playerMachine.playerInput.Player.Jump.Disable();
        playerMachine.playerInput.Player.Jump.performed -= PlayerJump;
        playerMachine.PlayerNearClimbArea -= OnPlayerNearClimbArea;
    }

}
