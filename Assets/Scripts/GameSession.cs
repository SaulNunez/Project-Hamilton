using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public string character;

    void Start()
    {
        Socket.Instance?.RegisterListener(this, UpdateCharacterList, "available_character");
    }

    void Update()
    {
        
    }

    void UpdateCharacterList(string data)
    {

    }

    private void OnDestroy()
    {
        Socket.Instance?.RemoveListener(UpdateCharacterList);
    }
}
