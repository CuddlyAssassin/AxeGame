using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FPSController))]
public class test_shooting : MonoBehaviour {

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
    private Rigidbody axeHoming;
    [SerializeField]
    private FPSController _jump;

    bool thrown;
    bool noCD;
    bool triShot;
    bool immune;
    bool homing;
#endregion
    void Start()
    {
        thrown = false;
        noCD = false;
        triShot = false;
        immune = false;
        homing = false;
        FPSController _jump = gameObject.GetComponent<FPSController>();
    }

    void Update()
    {
        if (triShot == false && homing == false)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && thrown == false)
            {
                Fire();
            }
        }

        if (triShot == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && thrown == false)
            {
                ThreeFire();
            }
        }

        if (homing == true)
        {
            if (Input.GetKey(KeyCode.Mouse0) && thrown == false)
            {
                HomingFire();
            }
        }
    }

    void Fire()
    {
        if (noCD == false)
        {
            axe.SetActive(false);
            Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            bulletClone.velocity = spawnPoint.transform.forward * bulletSpeed;
            thrown = true;
            Invoke("AxeReset", reset);
        }

        else if (noCD == true)
        {
            axe.SetActive(false);
            Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            bulletClone.velocity = spawnPoint.transform.forward * bulletSpeed;
        }
    }

    
    void ThreeFire()
    {
        axe.SetActive(false);
        Rigidbody bulletClone2 = (Rigidbody)Instantiate(bullet, spawnPoint.transform.position + spawnPoint.transform.right * 1.2f, spawnPoint.transform.rotation);
        Rigidbody bulletClone3 = (Rigidbody)Instantiate(bullet, spawnPoint.transform.position + spawnPoint.transform.right * -1.2f, spawnPoint.transform.rotation);
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        bulletClone.velocity = spawnPoint.transform.forward * bulletSpeed;
        bulletClone2.velocity = spawnPoint.transform.forward * bulletSpeed;
        bulletClone3.velocity = spawnPoint.transform.forward * bulletSpeed;
        thrown = true;
        Invoke("AxeReset", reset);
    }

    void HomingFire()
    {
        axe.SetActive(false);
        Rigidbody homingClone = (Rigidbody)Instantiate(axeHoming, spawnPoint.transform.position, spawnPoint.transform.rotation);
        //bulletClone.velocity = spawnPoint.transform.forward * bulletSpeed;
        thrown = true;
        Invoke("AxeReset", reset);
    }

    void Immunity()
    {
        gameObject.tag = "Untagged";
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
