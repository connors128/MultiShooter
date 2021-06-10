using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Connection;
using MLAPI.Transports.UNET;
using System;

public class ConnectionManager : MonoBehaviour
{
    public GameObject connectionButtonPanel;
    public Camera mainCamera;
    public string ipaddress = "127.0.0.1";
    UNetTransport transport;

    //happen on server
    public void Host()
    {           
        connectionButtonPanel.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(StartSpawnPos(), Quaternion.identity); //spawn manager later
    }

    //happen on server
    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        //Debug.Log("Approving a connection");
        //check the incoming data
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "Password1234"; //check if password is correct for the connection
        callback(true, null, approve, StartSpawnPos(), Quaternion.identity);
    }

    public void Join()
    {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipaddress;

        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Password1234"); //why is this important?? Encodes password as byte array
        NetworkManager.Singleton.StartClient();

            connectionButtonPanel.SetActive(false);
            mainCamera.gameObject.SetActive(false);
            //Debug.Log(NetworkManager.Singleton.ConnectedClientsList);
  
    }


    Vector3 StartSpawnPos()
    {
        float x = UnityEngine.Random.Range(-10f, 10f);
        //float y = UnityEngine.Random.Range(-10f, 10f);
        float z = UnityEngine.Random.Range(-10f, 10f);
        return new Vector3(x, 1, z);
    }

    public void IPAdressChanged(string newAddress)
    {
        this.ipaddress = newAddress;
    }

}
