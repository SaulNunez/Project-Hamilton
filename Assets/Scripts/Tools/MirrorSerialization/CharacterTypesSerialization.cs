using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class CharacterTypesSerialization
{
    public static void WriteCharacterTypes(this NetworkWriter writer, CharacterTypes characterType)
    {
        writer.WriteInt((int)characterType);
    }

    public static CharacterTypes ReadCharacterTypes(this NetworkReader reader)
    {
        return (CharacterTypes)reader.ReadInt();
    }
}

