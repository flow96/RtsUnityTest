using RtsNetworkingLibrary.networking.manager;
using RtsNetworkingLibrary.networking.messages.@base;
using UnityEngine;

public class ChatParser : MonoBehaviour
{
    public void HandleServerMessage(NetworkMessage message)
    {
        if(message is ChatMessage)
            NetworkManager.Instance.TcpServerSendBroadcast(message);
    }

    public void HandleClientMessage(NetworkMessage message)
    {
        if (message is ChatMessage)
            GameObject.Find("ChatContent").GetComponent<ChatHandler>().AddMessage((ChatMessage)message);
    }
}
