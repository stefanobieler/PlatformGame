using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpState : BaseState
{
    private PlayerStateMachine playerMachine;
    private Rigidbody2D playerRigidBody;
    private InputAction moveInput;
    private float moveDir;

    public PlayerJumpState(BaseStateMachine stateMachine) : base(stateMachine)
    {
        playerMachine = stateMachine as PlayerStateMachine;
        playerRigidBody = playerMachine.playerRigidBody;
    }

    public override void Start()
    {
        base.Start();
        playerMachine.PlayerLandOnGround += OnLandOnGround;
        float currentXVelocity = playerRigidBody.velocity.x;
        playerRigidBody.velocity = new Vector2(currentXVelocity, playerMachine.jumpSpeed);
        moveInput = playerMachine.playerInput.Player.Move;
        moveInput.Enable();
    }

    public override void Update()
    {
        base.Update();
        moveDir = moveInput.ReadValue<float>();
        bool isMoving = Mathf.Abs(moveDir) > Mathf.Epsilon ? true : false;

        if(isMoving){
            playerMachine.transform.localScale = new Vector2(Mathf.Sign(moveDir), 1.0f);
        }

    }

    public override void FixedUpdate()
    {
        float currentYVelocity = playerRigidBody.velocity.y;
        playerRigidBody.velocity = new Vector2(moveDir * playerMachine.moveSpeed, currentYVelocity);
    }

    public override void Exit()
    {
        base.Exit();
        playerMachine.PlayerLandOnGround -= OnLandOnGround;
        moveInput.Disable();
    }

    private void OnLandOnGround(){
        previousState = this;
        nextState = playerMachine.states[(int)PlayerStateMachine.PlayerState.Idle];
        playerMachine.ChangeState(nextState);
    }

}
