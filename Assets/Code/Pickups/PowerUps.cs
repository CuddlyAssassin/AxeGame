using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUps : MonoBehaviour {

    [SerializeField]
    private GameObject pickupSound;

    [SerializeField]
    private float powerUpTimer;
    [SerializeField]
    private FPSController _jump;
    [SerializeField]
    private PlayerHealth _hp;
    [SerializeField]
    private PlayerShoot shotScript;

    bool immune = false;
    bool highJump = false;
    bool hpRestore = false;

    void Start()
    {
        _jump = gameObject.GetComponent<FPSController>();
        _hp = gameObject.GetComponent<PlayerHealth>();
        shotScript = gameObject.GetComponent<PlayerShoot>();
    }

    void PickUp()
    {
        GameObject sound = (GameObject)Instantiate(pickupSound, transform.position, transform.rotation);
        NetworkServer.Spawn(sound);
    }

    void OnTriggerEnter(Collider b)
    {
        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("NoCoolDown"))
        {
            NoCoolDown();
            PickUp();
            Destroy(b.gameObject);
            Invoke("PowerUpReset", powerUpTimer);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("TriShot"))
        {
            TrippleShot();
            PickUp();
            Destroy(b.gameObject);
            Invoke("PowerUpReset", powerUpTimer);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("HighJump"))
        {
            _jump.HighJump();
            PickUp();
            Destroy(b.gameObject);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("Immunity"))
        {
            gameObject.tag = "Untagged";
            PickUp();
            Destroy(b.gameObject);
            Invoke("PowerUpReset", powerUpTimer);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("Homing"))
        {
            HomingShot();
            PickUp();
            Destroy(b.gameObject);
            Invoke("PowerUpReset", powerUpTimer);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("RestoreHp"))
        {
            _hp.Heal();
            PickUp();
            Destroy(b.gameObject);
        }
    }

    void NoCoolDown()
    {
        shotScript._noCD = true;
    }

    void TrippleShot()
    {
        shotScript._triShot = true;
    }

    void HomingShot()
    {
        shotScript._homing = true;
        gameObject.tag = "Target";
    }

    void PowerUpReset()
    {
        shotScript._homing = false;
        shotScript._noCD = false;
        shotScript._triShot = false;
        _hp.immune = false;
        gameObject.tag = "Player";
    }

}
