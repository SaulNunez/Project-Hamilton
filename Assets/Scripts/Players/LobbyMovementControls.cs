using Assets.Scripts.Multiplayer.ServerRequestsPayload;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMovementControls : MonoBehaviour
{
    public GameObject playerBase;

    public class Player
    {
        public Player(string name, string playerName, Stats startingStats)
        {
            characterName = name;
            this.playerName = playerName;
            stats = startingStats;
        }

        public string characterName;
        public string playerName;
        public Stats stats;
        public int x;
        public int y;
        public int floor;
        public GameObject gameObject;
    }

    public List<Player> playersInLobby;

    [System.Serializable]
    public class ScreenCharacterInfo
    {
        public string Name;
        public Sprite CharacterSprite;
    }

    public List<ScreenCharacterInfo> characterInfo;
    
    void Start()
    {
        HamiltonHub.Instance.OnMoveUpdate += OnPositionUpdate;
        HamiltonHub.Instance.OnStatsUpdate += OnStatsUpdate;
        HamiltonHub.Instance.OnOtherPlayerSelectedCharacter += OnPlayerSelectionInLobby;
    }

    private void OnPlayerSelectionInLobby(NewPlayerInfo newPlayerInfo)
    {
        var player = new Player(newPlayerInfo.CharacterSelected, newPlayerInfo.LobbyName, newPlayerInfo.StartingStats);
        playersInLobby.Add(player);

        player.gameObject = Instantiate(playerBase, transform);

        var newCharacterInfo = characterInfo.Find(ci => ci.Name == newPlayerInfo.CharacterSelected);

        var spriteRendererInplayer = player.gameObject.GetComponent<SpriteRenderer>();
        spriteRendererInplayer.sprite = newCharacterInfo.CharacterSprite;
    }

    private void OnStatsUpdate(NewStats newStats)
    {
        var player = playersInLobby.Find(x => x.characterName == newStats.PlayerName);

        player.stats.Bravery = newStats.Stats.Bravery;
        player.stats.Intelligence = newStats.Stats.Intelligence;
        player.stats.Physical = newStats.Stats.Physical;
        player.stats.Sanity = newStats.Stats.Sanity;
    }

    private void OnPositionUpdate(MovementRequest moveInfo)
    {
        var player = playersInLobby.Find(x => x.characterName == moveInfo.Character);

        player.x = moveInfo.X;
        player.y = moveInfo.Y;
        player.floor = moveInfo.Floor;
    }

    void OnDestroy()
    {
        HamiltonHub.Instance.OnMoveUpdate -= OnPositionUpdate; 
        HamiltonHub.Instance.OnStatsUpdate -= OnStatsUpdate;
        HamiltonHub.Instance.OnOtherPlayerSelectedCharacter -= OnPlayerSelectionInLobby;
    }
}
