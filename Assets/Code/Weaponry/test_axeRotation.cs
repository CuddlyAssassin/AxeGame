using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_axeRotation : MonoBehaviour {

    [SerializeField]
    private float speed;


    void Awake()
    {
        Destroy(gameObject,10f);
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

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            print("hit");
            
            Destroy(gameObject);
        }

        if (c.gameObject.tag == "Target")
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

    void OnTriggerEnter(Collider b)
    {
        if (b.gameObject.gameObject.tag == "PickUp")
        {
            Destroy(b.gameObject);
            Destroy(gameObject);
        }
    }
}
