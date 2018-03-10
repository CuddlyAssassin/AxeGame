using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerSetup : NetworkBehaviour {

    public static bool NameTag = false;

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private Text playerName;

    Camera sceneCamera;

    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    [SerializeField]
    GameObject playerHPCanvas;
    private GameObject playerHPinstance;


    void LateUpdate()
    {
        CmdPlayerName();
        PlayerName();
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
