using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class PlayerManager : NetworkBehaviour
{
    public float remainingTime;
    public GameObject GameManagerGO, escapeMenuGO;
    public Text _text;
    public int currentRank;
    bool stopText = false;
    //public List<GameObject> playersWalls;
    public struct ConnectedPlayers
    {
        public bool isLoser;
        public GameObject _gameObject;
        public int rank;
        public float timeAlive;
    }
    static public List<ConnectedPlayers> playerArr = new List<ConnectedPlayers>();

    // Start is called before the first frame update
    void Start()
    {
        remainingTime = GameManagerGO.GetComponent<GameManager>().RoundTimer;
        _text = this.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        ConnectedPlayers thisPlayer = new ConnectedPlayers();
        thisPlayer.isLoser = false;
        thisPlayer._gameObject = this.gameObject;
        thisPlayer.rank = 0;
        playerArr.Add(thisPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        //NUMBER OF PLAYERS IN SESSION
        //Debug.Log("Players: " + playerArr.Count);
        float DamagePercent = this.GetComponent<PlayerHealth>().health.Value;
        float remaininglives = this.GetComponent<PlayerHealth>().LifeCount.Value;
        remainingTime -= Time.deltaTime;

        if (IsLocalPlayer && !stopText)
        {
            _text.text = DamagePercent + "% Lives: " + remaininglives + " Time: " + Mathf.Round(remainingTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _text.enabled = true;
                this.GetComponent<FPSMovement>().enabled = true;
                escapeMenuGO.SetActive(false);
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _text.enabled = false;
                this.GetComponent<FPSMovement>().enabled = false;
                escapeMenuGO.SetActive(true);
            }
        }
    }
    public void Disconnect()//wont work rn
    {
        if (IsHost)
        {
            NetworkManager.Singleton.StopHost();
        }
        else if (IsClient)
        {
            NetworkManager.Singleton.StopClient();
            
        }
        else if (IsServer)
        {
            NetworkManager.Singleton.StopServer();
        }
        Application.Quit();
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void setAliveTime()
    {
        for(int i = 0; i < playerArr.Count; i++)
        {
            if(playerArr[i]._gameObject == this.gameObject)
            {
                ConnectedPlayers thisPlayer = new ConnectedPlayers();

                thisPlayer = playerArr[i];
                thisPlayer.rank = currentRank;
                thisPlayer.isLoser = true;
                currentRank--;
                thisPlayer.timeAlive = remainingTime;
                playerArr[i] = thisPlayer;
            }
        }
        for (int i = 0; i < playerArr.Count; i++)
        {
            if (playerArr[i].rank == 0)
            {
                ConnectedPlayers thisPlayer = new ConnectedPlayers();

                thisPlayer = playerArr[i];
                thisPlayer.rank = 1;
                thisPlayer.isLoser = false;
                //thisPlayer.timeAlive = remainingTime;
                playerArr[i] = thisPlayer;
            }
        }
        if (currentRank < 3)
        {
            GameManagerGO.GetComponent<GameManager>().getLosers();
        }
    }

    [ClientRpc]
    public void setLoserTextClientRpc()
    {
        if(IsLocalPlayer)
        {
            this.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "YOU LOSE";
            this.gameObject.GetComponent<PlayerManager>().enabled = false;
            stopText = true;
        }

    }

    [ClientRpc]
    public void setWinnerTextClientRPC()
    {
        if(IsLocalPlayer)
        {
            this.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "YOU WIN";
            this.gameObject.GetComponent<PlayerManager>().enabled = false;
            stopText = true;
        }

    }
}
