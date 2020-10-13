using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ResultPayload
{
    public class AvailableMovementOptions
    {
        public bool CanGoUp { get; set; }
        public bool CanGoDown { get; set; }
        public bool CanGoLeft { get; set; }
        public bool CanGoRight { get; set; }
        public bool CanGoDownFloor { get; set; }
        public bool CanGoUpFloor { get; set; }
    }
}
