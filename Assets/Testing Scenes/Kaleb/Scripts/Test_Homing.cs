using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Homing : MonoBehaviour {

    [SerializeField]
    private Transform target;
    public Transform myTransform;

    public float speed = 18;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

	// Update is called once per frame
	void Update () {

        if (target != null)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }else
        {
            Destroy(gameObject);
        }
	}
}
