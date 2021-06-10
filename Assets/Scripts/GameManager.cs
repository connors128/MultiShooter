using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    struct losers{
        public bool isLoser;
        public GameObject _gameObject;
    }
    List<losers> losersArr = new List<losers>();
    float quitTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = RoundTimer;
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
        if(NetworkManager.ConnectedClientsList.Count > 3)
        {
            if(NetworkManager.ConnectedClientsList[0].PlayerObject.gameObject.GetComponent<PlayerHealth>().LifeCount.Value == 0)
            {
                winnerGO = NetworkManager.ConnectedClientsList[1].PlayerObject.gameObject;
                winnerGO.GetComponent<PlayerManager>()._text.text = "YOU WIN";
                NetworkManager.ConnectedClientsList[0].PlayerObject.gameObject.GetComponent<PlayerManager>()._text.text = "YOU LOSE";
            }
            else
                winnerGO = NetworkManager.ConnectedClientsList[0].PlayerObject.gameObject;
            gameOver = true;
        }
        //else if()
        //{
        //    for (int i = 0; i < NetworkManager.ConnectedClientsList.Count; i++)
        //    {
        //        GameObject tempGO = NetworkManager.ConnectedClientsList[i].PlayerObject.gameObject;
        //
        //        if (tempGO.GetComponent<PlayerHealth>().LifeCount.Value == 0)
        //        {
        //            tempLo._gameObject = NetworkManager.ConnectedClientsList[i].PlayerObject.gameObject;
        //            tempLo.isLoser = true;
        //            losersArr.Add(tempLo);
        //        }
        //       else if (tempGO.GetComponent<PlayerHealth>().LifeCount.Value == 2)
        //        {
        //            tempLo._gameObject = NetworkManager.ConnectedClientsList[i].PlayerObject.gameObject;
        //            tempLo.isLoser = true;
        //            losersArr.Add(tempLo);
        //        }
        //       else if (tempGO.GetComponent<PlayerHealth>().LifeCount.Value == 3)
        //        {
        //            tempLo._gameObject = NetworkManager.ConnectedClientsList[i].PlayerObject.gameObject;
        //            tempLo.isLoser = true;
        //            losersArr.Add(tempLo);
        //        }
        //    }
        //}

    }
}
