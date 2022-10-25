using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController2D controller;

    [SerializeField] float runSpd;
    private Vector2 movement;

    private bool jump;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        controller.Move(movement.x * runSpd * 10 * Time.deltaTime, false, jump);
        jump = false;
    }

    public void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jump = true;
        }

    } 

}
