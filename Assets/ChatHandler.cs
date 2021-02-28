using System.Collections;
using System.Collections.Generic;
using RtsNetworkingLibrary.networking.messages.@base;
using UnityEngine;
using UnityEngine.UI;

public class ChatHandler : MonoBehaviour
{
    public GameObject chatMessagePrefab;
    
    public void AddMessage(ChatMessage message)
    {
        GameObject g = Instantiate(chatMessagePrefab, transform);
        g.GetComponent<Text>().text = message.playerInfo.username + ": " + message.messageText;
    }
}
