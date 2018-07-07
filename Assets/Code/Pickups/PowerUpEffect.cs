using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{

    public float rotSpeed;

    float bounceSpeed = 0.006f;

    bool _up = true;
    bool _down;

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);

        Bounce();

        if (Input.GetKeyDown(KeyCode.M))
        {
            print(transform.position.y);
        }

        if (0.5 >= transform.position.y)
        {
            _up = false;
            _down = true;
        }

        if (-0.5 >= transform.position.y)
        {
            _down = false;
            _up = true;
        }
    }

    void Bounce()
    {
        if (_up == true)
        {
            transform.position += transform.up * Time.deltaTime;
        }

        if (_down == true)
        {
            transform.position -= transform.up * Time.deltaTime;
        }
    }
}
