using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ServerRequestsPayload
{
    public class NewStats
    {
        public string PlayerName { get; set; }
        public Stats Stats { get; set; }
    }
}
