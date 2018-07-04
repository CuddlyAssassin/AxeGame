using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour {

    [SerializeField]
    private float puTimer;
    [SerializeField]
    private float puReset;
    [SerializeField]
    private FPSController _jump;
    [SerializeField]
    private PlayerHealth _hp;
    [SerializeField]
    private PlayerShoot shotScript;
    [SerializeField]
    private GameObject timerObj;

    public Text timerText;

    bool resetActivated = false;

    void Start()
    {
        _jump = gameObject.GetComponent<FPSController>();
        _hp = gameObject.GetComponent<PlayerHealth>();
        shotScript = gameObject.GetComponent<PlayerShoot>();
        puTimer = puReset;
    }

    void Update()
    {
        if (resetActivated == true)
        {
            puTimer -= Time.smoothDeltaTime;
            timerText.text = puTimer.ToString("f2");
            if (puTimer <= 0)
            {
                timerObj.SetActive(false);
                PowerUpReset();
                resetActivated = false;
                puTimer = puReset;
            }
        }
    }

    void OnTriggerEnter(Collider b)
    {
        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("NoCoolDown"))
        {
            NoCoolDown();
            Destroy(b.gameObject);
            resetActivated = true;
            timerObj.SetActive(true);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("TriShot"))
        {
            TrippleShot();
            Destroy(b.gameObject);
            resetActivated = true;
            timerObj.SetActive(true);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("HighJump"))
        {
            _jump.HighJump();
            Destroy(b.gameObject);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("Immunity"))
        {
            gameObject.tag = "Untagged";
            Destroy(b.gameObject);
            resetActivated = true;
            timerObj.SetActive(true);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("Homing"))
        {
            HomingShot();
            Destroy(b.gameObject);
            resetActivated = true;
            timerObj.SetActive(true);
        }

        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("RestoreHp"))
        {
            _hp.Heal();
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
