using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{

    public float rotSpeed;

    float bounceSpeed = 0.006f;
    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }
}
