using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : NetworkBehaviour {

    public static bool IsOn = false;

    [SerializeField]
    GameObject pauseMenu;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void LeaveRoom()
    {
        IsOn = false;
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.StopHost();
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
    }

    void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
