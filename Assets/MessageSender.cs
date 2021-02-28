using System.Collections;
using System.Collections.Generic;
using RtsNetworkingLibrary.networking.manager;
using UnityEngine;
using UnityEngine.UI;

public class MessageSender : MonoBehaviour
{
    public InputField inputField;

    public void SendMessage()
    {
        NetworkManager.Instance.TcpSendToServer(new ChatMessage(inputField.text.ToString().Trim()));
        inputField.text = "";
    }

}
