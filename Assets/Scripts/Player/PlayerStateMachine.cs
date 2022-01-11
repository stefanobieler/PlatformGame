using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : BaseStateMachine
{
    [SerializeField] public float moveSpeed { get; private set; } = 10.0f;
    [SerializeField] public float jumpSpeed { get; private set; } = 26.0f;
    [SerializeField] public float climbSpeed { get; private set; } = 5.0f;
    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Climb
    }
    public PlayerInputAction playerInput { get; private set; }

    public Animator anim { get; private set; }
    public Rigidbody2D playerRigidBody { get; private set; }
    public event Action PlayerLandOnGround;
    public event Action<bool> PlayerNearClimbArea;
    public float defaultGravityScale { get; private set; }
    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            PlayerLandOnGround?.Invoke();
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Climbing"))
        {
            PlayerNearClimbArea?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Climbing"))
        {
            PlayerNearClimbArea?.Invoke(false);
        }
    }

    private void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.layer == LayerMask.NameToLayer("Climbing")){
            PlayerNearClimbArea?.Invoke(true);
        }
    }

    private void Awake()
    {
        playerInput = new PlayerInputAction();
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        defaultGravityScale = playerRigidBody.gravityScale;
        AddStatesToDictionary();
    }

    private void AddStatesToDictionary()
    {
        states = new Dictionary<int, BaseState>();
        states.Add((int)PlayerState.Idle, new PlayerIdleState(this));
        states.Add((int)PlayerState.Run, new PlayerRunState(this));
        states.Add((int)PlayerState.Jump, new PlayerJumpState(this));
        states.Add((int)PlayerState.Climb, new PlayerClimbState(this));

    }


    protected override BaseState GetInitialState()
    {
        return new PlayerIdleState(this);
    }


}
