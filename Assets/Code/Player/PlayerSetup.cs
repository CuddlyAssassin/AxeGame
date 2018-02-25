using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentsToDisable;

    Camera sceneCamera;

    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance;

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
