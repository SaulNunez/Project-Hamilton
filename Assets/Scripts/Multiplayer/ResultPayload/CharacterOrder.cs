using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ResultPayload
{
    public class CharacterOrder
    {
        public string CharacterName { get; set; }
        public int TurnThrow { get; set; }
        public int TurnOrder { get; set; }
    }
}
