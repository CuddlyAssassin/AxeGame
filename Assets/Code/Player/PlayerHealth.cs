using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class PlayerHealth : NetworkBehaviour {

    public int _amount = 50;
    public int _hpGive = 50;
    [SyncVar]
    bool immune = false;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Immunity()
    {
        if (immune == false)
        {
            immune = true;
        }
        else
        {
            immune = false;
        }
    }

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    } 

    void OnCollisionEnter(Collision b)
    {
        if (b.gameObject.gameObject.layer == LayerMask.NameToLayer("Axe") && immune == false)
        {
            TakeDamage();
        }
    }

    
    public void TakeDamage()
    {
        if (!isServer)
            return;

        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " Now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            RpcDie();
        }
    }

    public void Heal()
    {
        if (!isServer)
            return;

        if(currentHealth != 100)
        {
            currentHealth += _hpGive;
        }
    }

    [ClientRpc]
    private void RpcDie()
    {
        if (!isLocalPlayer)
            return;

        isDead = true;

        gameObject.tag = "Untagged";

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        CharacterController _col = GetComponent<CharacterController>();
        if (_col != null)
            _col.enabled = false;

        Debug.Log(transform.name + " is DEAD!");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        gameObject.tag = "Player";
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        CharacterController _col = GetComponent<CharacterController>();
        if (_col != null)
            _col.enabled = true;
    }
}
