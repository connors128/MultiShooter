using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class PlayerShoot : NetworkBehaviour{
 
    
    public float fireRate = 10f, shootTimer = 0f, damageScale = 15f;
    public ParticleSystem bulletParticleSystem;
    ParticleSystem.EmissionModule em;
    NetworkVariableBool shooting = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly}, false);
    public GameObject wallPrefab;
    // Use this for initialization
    void Start () {
        em = bulletParticleSystem.emission;
    }
   
    // Update is called once per frame
    void Update () {
        if(IsLocalPlayer)
        {
            shooting.Value = Input.GetMouseButton(0);
            shootTimer += Time.deltaTime;
            if(shooting.Value && shootTimer >= 1/fireRate)
            {
                shootTimer = 0;
                //call method
                ShootServerRpc();
            }
        }
        em.rateOverTime = shooting.Value ? fireRate : 0f;

    }
    //these run on server and are called by client    client -> server

    //Server -> Client
    [ServerRpc]
    void ShootServerRpc()
    {
        Ray ray = new Ray(bulletParticleSystem.transform.position, bulletParticleSystem.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            var player = hit.collider.GetComponent<PlayerHealth>();
            if (player)
            {
                player.TakeDamage(damageScale);
                player.GetComponent<CharacterController>().SimpleMove(ray.direction * player.GetComponent<PlayerHealth>().health.Value * 2);
            }
            else if (hit.collider.gameObject.tag == "Floor")
            {
                Vector3 blockPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                Instantiate(wallPrefab, blockPos, Quaternion.identity);
            }
        }

        ShootClientRpc();
    }

    [ClientRpc]
    void ShootClientRpc()
    {
        Ray ray = new Ray(bulletParticleSystem.transform.position, bulletParticleSystem.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            var player = hit.collider.GetComponent<PlayerHealth>();
            if (player)
            {
                player.GetComponent<CharacterController>().SimpleMove(ray.direction * player.GetComponent<PlayerHealth>().health.Value * 2);
            }
            else if (hit.collider.gameObject.tag == "Floor")
            {
                Vector3 blockPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                Instantiate(wallPrefab, blockPos, Quaternion.identity);
            }
        }
    }    
}
