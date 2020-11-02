using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ResultPayload
{
    public class UniqueItemResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool NeedsThrow { get; set; } = false;
        public Stats StatEffects { get; set; }
        public bool SingleUse { get; set; } = false;
        public bool AffectsOtherPlayer { get; set; } = false;
    }
}
