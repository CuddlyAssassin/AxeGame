using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;


public class PlayerHealth : NetworkBehaviour {

    public int _amount = 50;
    public int _hpGive = 50;
    [SyncVar]
    public bool immune = false;

    [SyncVar]
    float respawnTimer;

    float resTime = 3;

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

    public GameObject deathCanvas;
    public Text deathTimer;

    public GameObject playerCanvas;
    public Text hpText;

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
        deathTimer.text = respawnTimer.ToString();
    }

    void LateUpdate()
    {
        if (respawnTimer <= 0)
        {
            Respawn();
        }

        if (currentHealth <= 0)
        {
            respawnTimer -= Time.smoothDeltaTime;
            deathTimer.text = "Respawning in: " + respawnTimer.ToString("f2");
        }
    }

    public void TakeDamage()
    {

        if (isDead)
            return;

        currentHealth -= _amount;

        hpText.text = "Hp: " + currentHealth.ToString();

        Debug.Log(transform.name + " Now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal()
    {
        if(currentHealth != 100)
        {
            currentHealth += _hpGive;
            hpText.text = "Hp: " + currentHealth.ToString();
        }
    }

    private void Die()
    {
        if (!isLocalPlayer)
            return;

        isDead = true;

        hpText.text = "Hp: 0";

        deathCanvas.SetActive(true);

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

    [Command]
    void CmdRespawn()
    {
        SetDefaults();
        deathCanvas.SetActive(false);
        Transform _spawn = (spawnPoints[Random.Range(0, spawnPoints.Length)]);
        transform.position = _spawn.transform.position;
        transform.rotation = _spawn.transform.rotation;
        gameObject.tag = "Player";
        respawnTimer = resTime;
        deathTimer.text = respawnTimer.ToString();
    }

    void Respawn()
    {
        SetDefaults();
        deathCanvas.SetActive(false);
        Transform _spawn = (spawnPoints[Random.Range(0, spawnPoints.Length)]);
        transform.position = _spawn.transform.position;
        transform.rotation = _spawn.transform.rotation;
        gameObject.tag = "Player";
        respawnTimer = resTime;
        deathTimer.text = respawnTimer.ToString();
        CmdRespawn();
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        hpText.text = "Hp: " + currentHealth.ToString("f0");

        CharacterController _col = GetComponent<CharacterController>();
        if (_col != null)
            _col.enabled = true;
    }
}
