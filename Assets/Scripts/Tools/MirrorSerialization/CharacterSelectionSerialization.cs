using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class CharacterSelectionSerialization
{
    public static void WriteCharacterSelection(this NetworkWriter writer, 
        AvailableCharactersMemory.CharacterSelection characterType)
    {
        writer.WriteCharacterTypes(characterType.characterSelected);
        writer.WriteString(characterType.characterName);
    }

    public static AvailableCharactersMemory.CharacterSelection ReadPCharacterSelection(this NetworkReader reader)
    {
        return new AvailableCharactersMemory.CharacterSelection(reader.ReadCharacterTypes(), reader.ReadString());
    }
}

