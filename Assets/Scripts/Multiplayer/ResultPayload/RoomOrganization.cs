using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ResultPayload
{
    public class RoomOrganization
    {
        public List<RoomPosition> MainFloor { get; set; }
        public List<RoomPosition> Basement { get; set; }
        public List<RoomPosition> TopFloor { get; set; }
    }
}
