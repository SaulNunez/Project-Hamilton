using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobby : MonoBehaviour
{
    public InputField codeInput;
    public Button sendToServer;

    public async void TryToJoinLobby()
    {
        sendToServer.interactable = false;
        
        sendToServer.interactable = true;
        
    }
}
