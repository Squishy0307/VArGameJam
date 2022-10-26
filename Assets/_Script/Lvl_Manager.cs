using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl_Manager : MonoBehaviour
{
    public Transform[] SpawnPoints;

    private int playersCount;

    void Start()
    {
        playersCount = PlayersManager.Instance.players.Count;

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            PlayersManager.Instance.players[i].transform.position = SpawnPoints[i].position;
            PlayersManager.Instance.enablePlayerControlls();
        }
    }

}
