using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobby : MonoBehaviour
{
    public InputField codeInput;
    public Button sendToServer;
    
    void Start()
    {
        Socket.Instance?.RegisterListener(this, (data) =>
        {
            gameObject.SetActive(false);
            sendToServer.interactable = false;
        }, "joined_lobby");
    }

    public void TryToJoinLobby()
    {
        var lobby = new Models.JoinLobby(codeInput.text);
        Socket.Instance?.Send("goto_lobby", JsonUtility.ToJson(lobby));

        StartCoroutine(WaitForLobbyResponse());
    }

    IEnumerator WaitForLobbyResponse()
    {
        yield return new WaitForSecondsRealtime(10);
    }
}
