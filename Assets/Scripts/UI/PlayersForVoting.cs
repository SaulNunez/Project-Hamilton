using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets up player buttons for the voting screen.
/// </summary>
public class PlayersForVoting : MonoBehaviour
{
    /// <summary>
    /// Button prefab, to be spawned
    /// </summary>
    [SerializeField]
    private GameObject playerButtonPrefab;

    /// <summary>
    /// Gameobject to attach the buttons
    /// </summary>
    [SerializeField]
    private GameObject playerButtonParent;

    /// <summary>
    /// Button to listen for skip
    /// </summary>
    [SerializeField]
    private Button skipButton;

    void OnEnable()
    {
        var players = GameObject.FindGameObjectsWithTag(Tags.Player);

        //Quitar jugadores existentes
        foreach (Transform child in playerButtonParent.transform)
        {
            if (child != this.transform)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var player in players)
        {
            var button = Instantiate(playerButtonPrefab, playerButtonParent.transform);

            //Obtener sprites
            var playerSprite = player.GetComponent<SpriteRenderer>().sprite;

            //Get name
            var playerName = player.GetComponentInChildren<PlayerName>().Name;

            //Get dead status
            var isDead = player.GetComponent<Die>().IsDead;

            var playerButton = button.GetComponent<PlayerVotingButton>();


            playerButton.PlayerSprite = playerSprite;
            playerButton.Name = playerName;
            playerButton.onSelect.AddListener(() => VoteForPlayer(player));
            button.GetComponent<Button>().interactable = !isDead;
        }

    }

    /// <summary>
    /// Utility vote casting method for the buttons.
    /// </summary>
    /// <param name="player"></param>
    void VoteForPlayer(GameObject player)
    {
        foreach (var button in playerButtonParent.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        skipButton.interactable = false;

        var votingManagerGameObject = GameObject.FindGameObjectWithTag(Tags.VotingManager);
        var votingManager = votingManagerGameObject.GetComponent<VotingManager>();

        votingManager.CmdCastVote(player);
    }

    public void SkipVoting()
    {
        foreach (var button in playerButtonParent.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        skipButton.interactable = false;

        var votingManagerGameObject = GameObject.FindGameObjectWithTag(Tags.VotingManager);
        var votingManager = votingManagerGameObject.GetComponent<VotingManager>();

        votingManager.CmdCastVote(null);
    }
}
