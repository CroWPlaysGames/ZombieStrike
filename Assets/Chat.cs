using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public string username;
    private readonly List<Message> messageList = new();
    [SerializeField] private int maxMessages;
    [SerializeField] private GameObject chatPanel, textObject;
    [SerializeField] private InputField messageInput;
    [Header("Chat Colours")]
    [SerializeField] private Color player;
    [SerializeField] private Color info;


    void Update()
    {
        if (!messageInput.text.Equals(""))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(username + ": " + messageInput.text, Message.MessageType.player);
                messageInput.text = "";
                messageInput.gameObject.SetActive(false);
            }
        }

        else
        {
            if (!messageInput.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                messageInput.gameObject.SetActive(true);
                messageInput.ActivateInputField();
                FindAnyObjectByType<PlayerController>().messaging = true;
            }
        }

        if (!messageInput.isFocused)
        {
            FindAnyObjectByType<PlayerController>().messaging = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("Added message", Message.MessageType.info);
            }
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messageList.Count.Equals(maxMessages))
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new() {text = text};
        GameObject NewTextObject = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = NewTextObject.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColour(messageType);
        messageList.Add(newMessage);
    }

    Color MessageTypeColour(Message.MessageType messageType)
    {
        Color colour = info;
        switch(messageType)
        {
            case Message.MessageType.player:
                colour = player;
                break;
        }
        return colour;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        player,
        info
    }
}