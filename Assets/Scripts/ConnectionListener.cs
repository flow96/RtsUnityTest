using RtsNetworkingLibrary.networking;
using RtsNetworkingLibrary.networking.manager;
using RtsNetworkingLibrary.unity.callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionListener : MonoBehaviour, IClientListener
{
    private void Start()
    {
        NetworkManager.Instance.AddClientListener(this);
    }

    public void OnConnected()
    {
        Debug.Log("On Connected");
        SceneManager.LoadScene(1);
    }

    public void OnDisconnected()
    {
        Debug.Log("On Disconnect");
    }

    public void OtherPlayerConnected(PlayerInfo playerInfo)
    {
        
    }

    public void OtherPlayerDisconnected(PlayerInfo playerInfo)
    {
    }
}
