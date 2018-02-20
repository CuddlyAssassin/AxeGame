using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class test_axeRotation : NetworkBehaviour {

    [SerializeField]
    private float speed;

    bool hitActive;

    void Awake()
    {
        Destroy(gameObject,10f);
        Invoke("HitActive", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        WeaponThrow();
    }

    void WeaponThrow()
    {
        transform.Rotate(speed * Time.deltaTime, 0, 0);
    }

    void HitActive()
    {
        hitActive = true;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player" && hitActive == true)
        {
            print("hit");
            //Destroy(c.gameObject);
            Destroy(gameObject);
        }

        else if (c.gameObject.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);
        }
    }
}
