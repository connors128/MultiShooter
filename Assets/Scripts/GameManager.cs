using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class GameManager : NetworkBehaviour
{
    public float RoundTimer = 120f;
    private float timeLeft;
    public bool gameOver = false;
    GameObject winnerGO;
    float quitTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = RoundTimer;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft < 1 || gameOver) //get an update on if a live has gone to 0
        {
            quitTime -= Time.deltaTime;
            //exit conditions here
        }
        if(quitTime == 0)
        {
            Application.Quit();
        }
    }

    public void getLosers()
    {
        //Debug.Log("Start of GameManager is Over");

        for (int i = 0; i < PlayerManager.playerArr.Count; i++)
        {
            PlayerManager.ConnectedPlayers tempPlayer = PlayerManager.playerArr[i];
            if (tempPlayer.rank == 1)
            {
                winnerGO = tempPlayer._gameObject;
                winnerGO.GetComponent<PlayerManager>().setWinnerTextClientRPC();
                //winnerGO.GetComponent<PlayerManager>().enabled = false;
                //winnerGO.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "YOU WIN";
            }
            else if(tempPlayer.rank > 1)
            {
                PlayerManager.playerArr[i]._gameObject.GetComponent<PlayerManager>().setLoserTextClientRpc();
            }
        }
        gameOver = true;
        //Debug.Log("Game is Over");
    }
}
