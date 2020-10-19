using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ServerRequestsPayload
{
    public class NewPlayerInfo
    {
        public string CharacterSelected { get; set; }
        public string LobbyName { get; set; }
        public Stats StartingStats { get; set; }
    }
}
