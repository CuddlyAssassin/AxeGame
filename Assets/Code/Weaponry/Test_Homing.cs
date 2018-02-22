using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Homing : MonoBehaviour {

    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private Transform target;
    public Transform myTransform;

    public float speed = 18;

    void Start()
    {
        _target = GameObject.FindWithTag("Player");
        if (_target == null)
            Destroy(gameObject);
        if(_target != null)
        target = _target.transform;
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
