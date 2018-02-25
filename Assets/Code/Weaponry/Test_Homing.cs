using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Homing : MonoBehaviour
{

    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private Transform target;
    public Transform myTransform;

    [SerializeField]
    private float rotSpeed;

    public float speed = 18;

    public GameObject child;

    void Start()
    {
        Destroy(gameObject, 10f);

        _target = GameObject.FindWithTag("Player");
        if (_target == null)
            Destroy(gameObject);
        if (_target != null)
            target = _target.transform;
    }
    // Update is called once per frame
    void Update()
    {
        child.transform.Rotate(rotSpeed * Time.deltaTime, 0, 0);

        if (target != null)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
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
