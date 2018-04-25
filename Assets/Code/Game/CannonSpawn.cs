using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CannonSpawn : NetworkBehaviour {

    [SerializeField]
    private float cannonShootSpeed;
    [SerializeField]
    private float cannonReset;
    [SerializeField]
    private float resetTimer;

    [SerializeField]
    private GameObject cannonBall;
    [SerializeField]
    private GameObject spawnPoint;

    bool _shot = false;

    private AudioSource fired;

    // Use this for initialization
    void Start () {
        resetTimer = cannonReset;
        fired = gameObject.GetComponent<AudioSource>();
	}
	
    void Update()
    {
        if (_shot == true)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
            {
                _shot = false;
                resetTimer = cannonReset;
            }
        }
    }

    void OnTriggerStay()
    {
        if (_shot == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CmdFire();
            }
        }
    }

    [Command]
    void CmdFire()
    {
        _shot = true;
        fired.Play();
        GameObject CannonClone = Instantiate(cannonBall, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody force = CannonClone.GetComponent<Rigidbody>();
        force.velocity = spawnPoint.transform.forward * cannonShootSpeed;
        NetworkServer.Spawn(CannonClone);
    }
}
