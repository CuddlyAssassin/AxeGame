using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListPartent;

    public string _customIP;

    private NetworkManager networkManager;

    bool clicked = false;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RefreshRoomList()
    {
        if (clicked == false)
        {
            clicked = true;
            ClearRoomList();
            networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
            status.text = "Loading...";
        }
        
    }

    public void LanClient()
    {
        networkManager.StartClient();
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";

        clicked = false;

        if (!success || matchList == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }

        foreach(MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListPartent);
            _roomListItemGO.transform.localScale = new Vector3(1, 1, 1);

            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if (_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(_roomListItemGO);
        }

        if (roomList.Count == 0)
        {
            status.text = "No rooms at the moment...";
        }
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void CustomIP(string _IP)
    {
        _customIP = _IP;
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "", _customIP, "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Joining Match...";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
