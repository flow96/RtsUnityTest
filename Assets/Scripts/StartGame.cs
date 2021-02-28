using RtsNetworkingLibrary.networking.manager;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    public void OnStartGame()
    {
        NetworkManager.Instance.StartGame("Game1");
    }
}
