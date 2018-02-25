using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RespawnPickup : NetworkBehaviour {

    public GameObject[] pickUps;

    public float delay = 1f;

    [SyncVar]
    bool respawn;

    public override void OnStartServer ()
    {
        Spawning();
    } 

    void Update()
    {
        if (transform.childCount == 0 && respawn == false)
        {
            respawn = true;
            Invoke("Spawning", delay);
        }

        if(transform.childCount == 1)
        {
            respawn = false;
        }
        
    }

    void Spawning()
    {
        GameObject _pickUp = Instantiate(pickUps[Random.Range(0, pickUps.Length)], gameObject.transform.position, gameObject.transform.rotation);
        _pickUp.transform.parent = gameObject.transform;
        NetworkServer.Spawn(_pickUp);
    }
}