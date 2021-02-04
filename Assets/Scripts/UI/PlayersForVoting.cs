using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersForVoting : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerButtonPrefab;

    [SerializeField]
    private GameObject playerButtonParent;

    [SerializeField]
    private Button skipButton;

    void OnEnable()
    {
        var players = GameObject.FindGameObjectsWithTag(Tags.Player);
        print($"Players: ${players.Length}");

        //Quitar jugadores existentes
        foreach (Transform child in playerButtonParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var player in players)
        {
            var button = Instantiate(playerButtonPrefab, playerButtonParent.transform);

            //Obtener sprites
            var playerSprite = player.GetComponent<SpriteRenderer>().sprite;

            //Get name
            var playerName = player.GetComponentInChildren<PlayerName>().playerName;

            var playerButton = button.GetComponent<PlayerVotingButton>();

            playerButton.PlayerSprite = playerSprite;
            playerButton.Name = playerName;
        }
    }
}
