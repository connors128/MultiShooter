using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariableFloat health = new NetworkVariableFloat(0f),
        LifeCount = new NetworkVariableFloat(3f);
    public float StartingHealth = 0f;
    public float CurrentHealth;
    public GameObject ConnectionGO;
    private void Start()
    {
        LifeCount.Value = 3f;
    }
    void FixedUpdate()
    {
        CurrentHealth = health.Value;
        if (this.transform.position.y < -10f || this.transform.position.y > 1500f)
        {
            this.GetComponent<FPSMovement>().enabled = false;
            this.GetComponent<CharacterController>().enabled = false;
            this.transform.position = RespawnPos();
            this.GetComponent<CharacterController>().enabled = true;
            this.GetComponent<FPSMovement>().enabled = true;
            RespawnServerRpc();
        }
        if(this.LifeCount.Value == 0)
        {
            this.GetComponent<PlayerManager>().GameManagerGO.GetComponent<GameManager>().getLosers();
        }
    }

    //running on the server
    public void TakeDamage(float damage)
    {
        health.Value += damage;
    }

    [ServerRpc]
    void RespawnServerRpc()
    {
        health.Value = 0; //need to make server rpc for this
        LifeCount.Value--;
    }

    Vector3 RespawnPos()
    {
        float x = UnityEngine.Random.Range(-10f, 10f);
        //float y = UnityEngine.Random.Range(-10f, 10f);
        float z = UnityEngine.Random.Range(-10f, 10f);
        return new Vector3(x, 1, z);
    }
}
