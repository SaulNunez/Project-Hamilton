using Mirror;
using System;
using System.Linq;

/// <summary>
/// To be used on lobby to keep track of the character players are using,
/// to only allow players to select a character nobody is using.   
/// So players can keep track visually of who is who.
/// </summary>
public class AvailableCharactersMemory : NetworkBehaviour
{
    public readonly struct CharacterSelection
    {
        public readonly CharacterTypes characterSelected;
        public readonly string characterName;

        public CharacterSelection(CharacterTypes characterSelected, string characterName)
        {
            this.characterSelected = characterSelected;
            this.characterName = characterName ?? throw new ArgumentNullException(nameof(characterName));
        }
    }

    readonly SyncList<CharacterSelection> selections = new SyncList<CharacterSelection>();

    public static event Action<CharacterTypes> OnCharacterAvailable;
    public static event Action<CharacterTypes> OnCharacterOccupied;

    public override void OnStartClient()                                       
    {
        base.OnStartClient();

        selections.Callback += OnSelectionsChanged;
    }

    public bool CharacterUsed(CharacterTypes selectedCharacter)
    {
        return selections.Any(x => x.characterSelected == selectedCharacter);
    }

    [Command(ignoreAuthority = true)]
    public void CmdSetPlayerSelection(string playerName, CharacterTypes selectedCharacter)
    {
        var previousCharacterSelection = selections.Find(x => x.characterName == playerName);
        if (!previousCharacterSelection.Equals(default(CharacterSelection)))
        {
            selections.Remove(previousCharacterSelection);
        }

        print($"Player name: {playerName}, character: {selectedCharacter}");

        selections.Add(new CharacterSelection(selectedCharacter, playerName));
    }

    private void OnSelectionsChanged(SyncList<CharacterSelection>.Operation op, 
        int itemIndex, CharacterSelection oldItem, CharacterSelection newItem)
    {
        if (op == SyncList<CharacterSelection>.Operation.OP_ADD || op == SyncList<CharacterSelection>.Operation.OP_INSERT)
        {
            OnCharacterOccupied?.Invoke(newItem.characterSelected);
            print($"Character occupied: {newItem.characterSelected}");
        }
        else if(op == SyncList<CharacterSelection>.Operation.OP_REMOVEAT)
        {
            OnCharacterAvailable?.Invoke(oldItem.characterSelected);
            print($"Character unoccupied: {newItem.characterSelected}");
        } else if(op == SyncList<CharacterSelection>.Operation.OP_SET)
        {
            print($"Character changed: {newItem.characterSelected}");
            OnCharacterAvailable?.Invoke(oldItem.characterSelected);
            OnCharacterOccupied?.Invoke(newItem.characterSelected);
        }

    }
}
