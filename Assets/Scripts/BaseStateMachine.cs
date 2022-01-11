using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{

    protected BaseState currentState;
    public Dictionary<int, BaseState> states { get; protected set; }

    void Start()
    {
        currentState = GetInitialState();
        currentState?.Start();
    }

    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    void LateUpdate()
    {
        currentState?.LateUpdate();
    }

    public void ChangeState(BaseState newState)
    {
        currentState?.Exit();

        currentState = newState;
        currentState?.Start();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

}
