using System;
using RtsNetworkingLibrary.networking.messages.@base;


[Serializable]
public class ChatMessage : NetworkMessage
{
    public readonly String messageText;
    public ChatMessage(string messageText)
    {
        this.messageText = messageText;
    }
}
