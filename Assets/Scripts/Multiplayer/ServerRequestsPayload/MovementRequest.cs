using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ServerRequestsPayload
{
    public class MovementRequest
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Floor { get; set; }
        public string Character { get; set; }
    }
}
