using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_shooting : MonoBehaviour {

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

    bool thrown;

    bool noCD;

    bool triShot;

    void Start()
    {
        thrown = false;
        noCD = false;
        triShot = false;
    }

    void Update()
    {
        if (triShot == false)
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

    void CoolDownReset()
    {
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
    }
}
