using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimbState : BaseState
{
    private PlayerStateMachine playerMachine;
    private InputAction moveInput;
    private InputAction climbInput;
    private Rigidbody2D playerRigidBody;
    private float climbSpeed;
    private float moveSpeed;
    private float climbDir;
    private float moveDir;


    public PlayerClimbState(BaseStateMachine stateMachine) : base(stateMachine)
    {
        playerMachine = stateMachine as PlayerStateMachine;
        playerRigidBody = playerMachine.playerRigidBody;
        climbInput = playerMachine.playerInput.Player.Climb;
        moveInput = playerMachine.playerInput.Player.Move;

        moveSpeed = playerMachine.moveSpeed / 2;
        climbSpeed = playerMachine.climbSpeed;

        playerMachine.PlayerNearClimbArea += OnPlayerNearClimbArea;
    }

    private void OnPlayerNearClimbArea(bool canClimb){
        if(!canClimb){
            ChangeToIdleState();
        }
    }

    private void ChangeToIdleState(){
        previousState = this;
        nextState = playerMachine.states[(int)PlayerStateMachine.PlayerState.Idle];
        playerMachine.ChangeState(nextState);
    }

    public override void Start()
    {
        base.Start();
        moveInput.Enable();
        climbInput.Enable();
        playerRigidBody.gravityScale = 0;
        playerMachine.anim.SetBool("isClimbing", true);
    }

    public override void Update()
    {
        base.Update();
        climbDir = climbInput.ReadValue<float>();
        moveDir = moveInput.ReadValue<float>();
        playerMachine.anim.speed = Mathf.Clamp(Mathf.Abs(playerRigidBody.velocity.y), 0.0f, 1f);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector2 newVelocity = new Vector2(moveDir * moveSpeed, climbDir * climbSpeed);
        playerRigidBody.velocity = newVelocity;
    }

    public override void Exit()
    {
        base.Exit();
        playerMachine.anim.SetBool("isClimbing", false);
        moveInput.Disable();
        climbInput.Disable();
        playerRigidBody.gravityScale = playerMachine.defaultGravityScale;
    }


}
