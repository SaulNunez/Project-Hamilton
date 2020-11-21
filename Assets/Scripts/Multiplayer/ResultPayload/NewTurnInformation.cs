using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ResultPayload
{
    public class NewTurnInformation
    {
        public bool CanThrowDiceForMovement { get; set; }
        public string CharacterId { get; set; }
        public Stats PlayerStats;
        public string DisplayName;
    }
}
