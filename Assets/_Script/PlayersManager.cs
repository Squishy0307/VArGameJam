using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance;

    public List<PlayerMovement> players = new List<PlayerMovement>();
    private List<bool> playersReady = new List<bool>();

    private int playerIndex = 1;

    public GameObject[] lobbyUi;

    public TextMeshProUGUI[] playersReadyTxt;
    public TextMeshProUGUI startGameTxt;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.gameObject.name = "Player_" + playerIndex;
        players.Add(playerInput.gameObject.GetComponent<PlayerMovement>());
        lobbyUi[playerIndex - 1].SetActive(true);
        players[playerIndex - 1].playerIndex = playerIndex;
        playerIndex++;

        playersReady.Add(false);
      
    }

    public bool checkAllPlayersReady()
    {
        if (playersReady.Count >= 2)
        {
            for (int i = 0; i < playersReady.Count; i++)
            {

                if (playersReady[i] == false)
                {
                    return false;
                }

            }

            return true;

        }
        return false;      
    }

    public void playerIsReady(int index)
    {
        playersReady[index - 1] = true;
        playersReadyTxt[index - 1].text = "Ready!";

        if(players.Count >= 2)
        {
            startGameTxt.text = "Hold Start to begin";
        }
    }

    public void LoadScene()
    {
        SceneChanger.Instance.changeScene("test");
    }

    public void enablePlayerControlls()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].changeCurrentControllInput("Controlls");
        }

    }
}
