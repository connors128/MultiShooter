using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class PlayerManager : NetworkBehaviour
{
    public float remainingTime;
    public GameObject GameManagerGO;
    public Text _text;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = GameManagerGO.GetComponent<GameManager>().RoundTimer;
        _text = this.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

    }

    // Update is called once per frame
    void Update()
    {
        float DamagePercent = this.GetComponent<PlayerHealth>().health.Value;
        float remaininglives = this.GetComponent<PlayerHealth>().LifeCount.Value;
        remainingTime -= Time.deltaTime;
        if(IsLocalPlayer && !GameManagerGO.GetComponent<GameManager>().gameOver)
            _text.text = DamagePercent + "% Lives: " + remaininglives + " Time: " + Mathf.Round(remainingTime);
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
