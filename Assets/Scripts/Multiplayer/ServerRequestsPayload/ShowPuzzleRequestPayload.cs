using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ServerRequestsPayload
{
    public class ShowPuzzleRequestPayload
    {
        public string Instructions { get; set; }
        public string Documentation { get; set; }
        public string InitialWorkspaceConfiguration { get; set; }
        public string PuzzleId { get; set; }
    }
}
