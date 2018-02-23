using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(FPSController))]
public class test_shooting : NetworkBehaviour {

    #region Data Information

    public int damage = 50;

    [SerializeField]
    private float delay;
    [SerializeField]
    private float bulletSpeed = 10;
    [SerializeField]
    private float reset;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject axeHoming;
    [SerializeField]
    private FPSController _jump;
    [SerializeField]
    private PlayerHealth _hp;

    bool thrown;
    bool noCD;
    bool triShot;
    bool immune;
    bool homing;

    [SyncVar]
    public GameObject axe;
    #endregion

    void Start()
    {
        thrown = false;
        noCD = false;
        triShot = false;
        immune = false;
        homing = false;
        FPSController _jump = gameObject.GetComponent<FPSController>();
        PlayerHealth _hp = gameObject.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
            if (triShot == false && homing == false)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && thrown == false)
                {
                    CmdFire();
                }
            }

            if (triShot == true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && thrown == false)
                {
                    CmdTri();
                }
            }

            if (homing == true)
            {
                if (Input.GetKey(KeyCode.Mouse0) && thrown == false)
                {
                    CmdHoming();
                }
            }
    }

    [Command]
    void CmdFire()
    {
        if (noCD == false)
        {
            axe.SetActive(false);
            GameObject bulletClone = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody force = bulletClone.GetComponent<Rigidbody>();
            force.velocity = spawnPoint.transform.forward * bulletSpeed;
            NetworkServer.Spawn(bulletClone);
            thrown = true;
            Invoke("AxeReset", reset);
        }

        else if (noCD == true)
        {
            axe.SetActive(false);
            GameObject bulletClone = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody force = bulletClone.GetComponent<Rigidbody>();
            force.velocity = spawnPoint.transform.forward * bulletSpeed;
            NetworkServer.Spawn(bulletClone);
        }
    }

    [Command]
    void CmdTri()
    {
        axe.SetActive(false);
        GameObject bulletClone2 = (GameObject)Instantiate(bullet, spawnPoint.transform.position + spawnPoint.transform.right * 1.2f, spawnPoint.transform.rotation);
        Rigidbody force2 = bulletClone2.GetComponent<Rigidbody>();
        force2.velocity = spawnPoint.transform.forward * bulletSpeed;
        NetworkServer.Spawn(bulletClone2);
        GameObject bulletClone3 = (GameObject)Instantiate(bullet, spawnPoint.transform.position + spawnPoint.transform.right * -1.2f, spawnPoint.transform.rotation);
        Rigidbody force3 = bulletClone3.GetComponent<Rigidbody>();
        force3.velocity = spawnPoint.transform.forward * bulletSpeed;
        NetworkServer.Spawn(bulletClone3);
        GameObject bulletClone = (GameObject)Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody force = bulletClone.GetComponent<Rigidbody>();
        force.velocity = spawnPoint.transform.forward * bulletSpeed;
        NetworkServer.Spawn(bulletClone);
        thrown = true;
        Invoke("AxeReset", reset);
    }

    [Command]
    void CmdHoming()
    {
        axe.SetActive(false);
        GameObject homingClone = (GameObject)Instantiate(axeHoming, spawnPoint.transform.position, spawnPoint.transform.rotation);
        NetworkServer.Spawn(homingClone);
        thrown = true;
        Invoke("AxeReset", reset);
    }

    void Immunity()
    {
        gameObject.tag = "Untagged";
        _hp.Immunity();
    }

    #region Reset Codes
    void CoolDownReset()
    {
        AxeReset();
        noCD = false;
    }

    void AxeReset()
    {
        axe.SetActive(true);
        thrown = false;
    }

    void TriReset()
    {
        triShot = false;
    }

    void ImmunityReset()
    {
        gameObject.tag = "Player";
        _hp.Immunity();
        immune = false;
    }

    void HomingReset()
    {
        homing = false;
        gameObject.tag = "Player";
    }
    #endregion

    #region Triggers
    void OnTriggerEnter(Collider b)
    {
        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("NoCoolDown"))
        {
            noCD = true;
            print("Rapid Fire");
            Destroy(b.gameObject);
            Invoke("CoolDownReset", delay);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("TriShot"))
        {
            triShot = true;
            print("Tripple Threat");
            Destroy(b.gameObject);
            Invoke("TriReset", delay);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("HighJump"))
        {
            _jump.HighJump();
            print("Higher Jumping");
            Destroy(b.gameObject);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("Immunity"))
        {
            immune = true;
            Immunity();
            print("Now Immune");
            Destroy(b.gameObject);
            Invoke("ImmunityReset", delay);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("Homing"))
        {
            homing = true;
            gameObject.tag = "Target";
            print("Homing Axes");
            Destroy(b.gameObject);
            Invoke("HomingReset", delay);
        }
    }
    #endregion
}
