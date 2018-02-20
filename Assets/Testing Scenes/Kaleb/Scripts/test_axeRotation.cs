using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_axeRotation : MonoBehaviour {

    [SerializeField]
    private float speed;

    bool hit;

    void Awake()
    {
        Destroy(gameObject,20f);
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

    void HitReset()
    {
        hit = false;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Enemies" && hit == false)
        {
            print("hit");
            Destroy(c.gameObject);
            Destroy(gameObject);
            hit = true;
            Invoke("HitReset", 0.2f);
        }

        else if (c.gameObject.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);
        }
    }
}
