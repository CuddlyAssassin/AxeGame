using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(FPSController))]
public class test_shooting : NetworkBehaviour {

    #region Data Information
    [SerializeField]
    private float delay;
    [SerializeField]
    private float bulletSpeed = 10;
    [SerializeField]
    private float reset;
    [SerializeField]
    private Rigidbody bullet;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject axe;
    [SerializeField]
    private FPSController _jump;
    
    bool thrown;

    bool noCD;

    bool triShot;

    bool immune;
#endregion
    void Start()
    {
        thrown = false;
        noCD = false;
        triShot = false;
        immune = false;
        FPSController _jump = gameObject.GetComponent<FPSController>();
    }

    void Update()
    {
        if (triShot == false)
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
                CmdThreeFire();
            }
        }
    }

    [Command]
    void CmdFire()
    {
        if (noCD == false)
        {
            //axe.SetActive(false);
            Rigidbody bulletClone = (Rigidbody)Network.Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation, 1);
            bulletClone.velocity = spawnPoint.transform.forward * bulletSpeed;
            thrown = true;
            Invoke("AxeReset", reset);
        }

        else if (noCD == true)
        {
            axe.SetActive(false);
            Rigidbody bulletClone = (Rigidbody)Network.Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation, 1);
            bulletClone.velocity = spawnPoint.transform.forward * bulletSpeed;;
        }
    }

    [Command]
    void CmdThreeFire()
    {
        axe.SetActive(false);
        Rigidbody bulletClone2 = (Rigidbody)Network.Instantiate(bullet, spawnPoint.transform.position + spawnPoint.transform.right * 1.2f, spawnPoint.transform.rotation,1);
        Rigidbody bulletClone3 = (Rigidbody)Network.Instantiate(bullet, spawnPoint.transform.position + spawnPoint.transform.right * -1.2f, spawnPoint.transform.rotation,1);
        Rigidbody bulletClone = (Rigidbody)Network.Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation,1);
        bulletClone.velocity = spawnPoint.transform.forward * bulletSpeed;
        bulletClone2.velocity = spawnPoint.transform.forward * bulletSpeed;
        bulletClone3.velocity = spawnPoint.transform.forward * bulletSpeed;
        thrown = true;
        Invoke("AxeReset", reset);
        NetworkServer.Spawn(axe);
    }

    void Immunity()
    {
        gameObject.tag = "Untagged";
    }

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
        immune = false;
    }

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
    }
}
