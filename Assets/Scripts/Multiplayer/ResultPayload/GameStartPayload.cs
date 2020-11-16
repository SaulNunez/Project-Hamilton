using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ResultPayload
{
    public class GameStartPayload
    {
        public List<CharacterOrder> PlayerOrder { get; set; }
        public RoomOrganization RoomPositions { get; set; }
    }
}
