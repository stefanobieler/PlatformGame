using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected BaseStateMachine stateMachine;
    protected BaseState previousState;
    protected BaseState nextState;
    protected string stateName = "Unkown";

    public BaseState(BaseStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }


    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void LateUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
