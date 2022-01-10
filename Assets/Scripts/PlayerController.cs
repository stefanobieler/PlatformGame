using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputAction playerInput;
    InputAction move;
    InputAction climb;

    private void Awake(){
        playerInput = new PlayerInputAction();
    }

    private void SetupInput(){
        //get the jump input
        playerInput.Player.Jump.performed += Jump;
        //Setup movement input
        move = playerInput.Player.Move;
        move.Enable();
        //set up climbing input
        climb = playerInput.Player.Climb;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        
    }

    private void OnEnable(){
        SetupInput();
    }
}
