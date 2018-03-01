using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeDamage : MonoBehaviour {

    [SerializeField]
    float axeRotationSpeed;

    void Awake()
    {
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        transform.Rotate(100 * axeRotationSpeed * Time.deltaTime, 0, 0);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerHealth>().TakeDamage();
            c.gameObject.GetComponent<AudioSource>().Play();
            print("hit");
            Destroy(gameObject);
        }

        if (c.gameObject.tag == "Target")
        {
            c.gameObject.GetComponent<PlayerHealth>().TakeDamage();
            c.gameObject.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }

        if (c.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider b)
    {
        if (b.gameObject.tag == "PickUp")
        {
            Destroy(b.gameObject);
            Destroy(gameObject);
        }
    }
}
