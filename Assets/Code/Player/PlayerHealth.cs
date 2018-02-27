using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class PlayerHealth : NetworkBehaviour {

    public int _amount = 50;
    public int _hpGive = 50;
    [SyncVar]
    public bool immune = false;

    [SyncVar]
    float respawnTimer;

    float resTime = 3f;

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

    public Transform[] spawnPoints;

    public void Immunity()
    {
        if (immune == false)
        {
            immune = true;
            print("health immune");
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
    
    void Start()
    {
        respawnTimer = resTime;
    }

    void Update()
    {
        if (respawnTimer <= 0)
        {
            RpcRespawn();
        }

        if (currentHealth <= 0)
        {
            respawnTimer -= Time.deltaTime;
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
    }

    [ClientRpc]
    void RpcRespawn()
    {
        SetDefaults();
        Transform _spawn = (spawnPoints[Random.Range(0, spawnPoints.Length)]);
        transform.position = _spawn.transform.position;
        transform.rotation = _spawn.transform.rotation;
        gameObject.tag = "Player";
        respawnTimer = resTime;
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
