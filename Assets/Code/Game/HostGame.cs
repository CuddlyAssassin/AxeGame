using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 4;

    private string roomName;

    public string customIP;

    [SerializeField]

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CustomIP(string _IP)
    {
        customIP = _IP;
    }

    public void CreateRoom()
    {
        if (roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players!");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", customIP, "", 0, 0, networkManager.OnMatchCreate);
            
        }
    }

    public void LocalCreateRoom()
    {
        networkManager.StartHost();
    }
}
