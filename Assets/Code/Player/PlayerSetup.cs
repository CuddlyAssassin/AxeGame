using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerSetup : NetworkBehaviour {

    public static bool NameTag = false;

    public static bool TabIsOn = false;

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private Text playerName;

    [SerializeField]
    private Text sensitivity;

    [SerializeField]
    public GameObject slider;

    Camera sceneCamera;

    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    PlayerHealth hp;

    void LateUpdate()
    {
        CmdPlayerName();
        PlayerName();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TabIsOn = true;
            slider.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            TabIsOn = false;
            slider.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i< componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
        }

        GetComponent<PlayerHealth>().Setup();
    }

    [Command]
    void CmdPlayerName()
    {
        playerName.text = transform.name;
        NameTag = false;
    }

    [Client]
    void PlayerName()
    {
        playerName.text = transform.name;
        NameTag = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager _player = GetComponent<PlayerManager>();

        TheGameManager.RetisterPlayer(_netID, _player);
    }

    void OnDisable()
    {
        Destroy(playerUIInstance);

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        TheGameManager.UnRegisterPlayer(transform.name);
    }

}
