using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public int playerIndex;

    private PlayerInput playerInput;
    private CharacterController2D controller;

    [SerializeField] float runSpd;
    private Vector2 movement;

    private bool jump;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController2D>();

        DontDestroyOnLoad (gameObject);
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

    public void OnAccept(InputValue value)
    {
        if (value.isPressed)
        {          
            PlayersManager.Instance.playerIsReady(playerIndex);
        }
    }

    public void OnStartGame(InputValue value)
    {
        if (PlayersManager.Instance.checkAllPlayersReady())
        {
            if (value.isPressed)
            {
                PlayersManager.Instance.LoadScene();
            }
        }
    }

    public void disableCurrentControlls()
    {
        playerInput.currentActionMap.Disable();
    }

    public void enableCurrentControlls()
    {
        playerInput.currentActionMap.Enable();
    }

    public void changeCurrentControllInput(string mapName)
    {
        playerInput.SwitchCurrentActionMap(mapName);
    }

    public void playerDed()
    {
        Debug.Log("PlayerIs Ded");
        gameObject.SetActive(false);
    }
}
