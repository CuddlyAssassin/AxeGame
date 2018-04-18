using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    #region Inspector Information
    [Header("Float Information")]
    [SerializeField]
    private float axeThrowSpeed;
    [SerializeField]
    private float axeReset;
    [SyncVar]
    public float resetTimer;

    [Header("Axe Information")]
    [SerializeField]
    private GameObject axeBullet;
    [SerializeField]
    private GameObject homingBullet;
    [SerializeField]
    private GameObject spawnPoint;

    [SyncVar]
    public GameObject axeVisual;

    [SyncVar]
    public bool _thrown = false;
    [SyncVar]
    public bool _noCD = false;
    [SyncVar]
    public bool _triShot = false;
    [SyncVar]
    public bool _homing = false;
    #endregion

    void Start()
    {
        resetTimer = axeReset;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (PauseMenu.IsOn || PlayerSetup.TabIsOn)
            return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (_triShot == false && _homing == false)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _thrown == false)
            {
                CmdAxeOff();
                AxeOff();
                axeVisual.SetActive(false);
                CmdFire();
                if (_noCD == false)
                {
                    _thrown = true;
                }
                    
            }
        }

        if (_triShot == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _thrown == false)
            {
                CmdAxeOff();
                AxeOff();
                axeVisual.SetActive(false);
                _thrown = true;
                CmdTri();
            }
        }

        if (_homing == true)
        {
            if (Input.GetKey(KeyCode.Mouse0) && _thrown == false)
            {
                CmdAxeOff();
                AxeOff();
                axeVisual.SetActive(false);
                _thrown = true;
                CmdHoming();
            }
        }

        if (_thrown == true)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
            {
                CmdAxeReset();
                AxeReset();
                _thrown = false;
                resetTimer = axeReset;
            }
        }

    }

    void AxeOff()
    {
        axeVisual.SetActive(false);
    }

    [Command]
    void CmdAxeOff()
    {
        axeVisual.SetActive(false);
        AxeOff();
    }

    void AxeReset()
    {
        axeVisual.SetActive(true);
        _thrown = false;
    }

    [Command]
    void CmdAxeReset()
    {
        axeVisual.SetActive(true);
        _thrown = false;
        AxeReset();
    }

    [Command]
    void CmdFire()
    {
        if (_noCD == false)
        {
            axeVisual.SetActive(false);
            _thrown = true;
            GameObject bulletClone = Instantiate(axeBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody force = bulletClone.GetComponent<Rigidbody>();
            force.velocity = spawnPoint.transform.forward * axeThrowSpeed;
            NetworkServer.Spawn(bulletClone);
        }

        else if (_noCD == true)
        {
            axeVisual.SetActive(false);
            GameObject bulletClone = Instantiate(axeBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody force = bulletClone.GetComponent<Rigidbody>();
            force.velocity = spawnPoint.transform.forward * axeThrowSpeed;
            NetworkServer.Spawn(bulletClone);
        }
    }

    [Command]
    void CmdTri()
    {
        axeVisual.SetActive(false);
        _thrown = true;
        GameObject bulletClone2 = (GameObject)Instantiate(axeBullet, spawnPoint.transform.position + spawnPoint.transform.right * 1.2f, spawnPoint.transform.rotation);
        Rigidbody force2 = bulletClone2.GetComponent<Rigidbody>();
        force2.velocity = spawnPoint.transform.forward * axeThrowSpeed;
        NetworkServer.Spawn(bulletClone2);
        GameObject bulletClone3 = (GameObject)Instantiate(axeBullet, spawnPoint.transform.position + spawnPoint.transform.right * -1.2f, spawnPoint.transform.rotation);
        Rigidbody force3 = bulletClone3.GetComponent<Rigidbody>();
        force3.velocity = spawnPoint.transform.forward * axeThrowSpeed;
        NetworkServer.Spawn(bulletClone3);
        GameObject bulletClone = (GameObject)Instantiate(axeBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody force = bulletClone.GetComponent<Rigidbody>();
        force.velocity = spawnPoint.transform.forward * axeThrowSpeed;
        NetworkServer.Spawn(bulletClone);
    }

    [Command]
    void CmdHoming()
    {
        axeVisual.SetActive(false);
        _thrown = true;
        GameObject homingClone = (GameObject)Instantiate(homingBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        NetworkServer.Spawn(homingClone);

    }

}
