using RtsNetworkingLibrary.networking;
using RtsNetworkingLibrary.networking.manager;
using RtsNetworkingLibrary.unity.callbacks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour, IClientListener
{

    public GameObject playerPrefab;
    
    private void Awake()
    {
        NetworkManager.Instance.AddClientListener(this);
    }

    private void Start()
    {
        foreach (var connectedPlayer in NetworkManager.Instance.ConnectedPlayers)
        {
            GameObject g = Instantiate(playerPrefab, this.transform);
            Text text = g.GetComponentInChildren<Text>();
            text.text = connectedPlayer.username;
        }
    }


    public void OnConnected()
    {
        
    }

    public void OnDisconnected()
    {
        
    }

    public void OtherPlayerConnected(PlayerInfo playerInfo)
    {
        GameObject g = Instantiate(playerPrefab, this.transform);
        Text text = g.GetComponentInChildren<Text>();
        text.text = playerInfo.username;
    }

    public void OtherPlayerDisconnected(PlayerInfo playerInfo)
    {
        
    }
}
