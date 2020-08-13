using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer.ResultPayload
{
    [Serializable]
    class EventData
    {
        public string type;
        public object payload;
        public string error = null;
    }

    [Serializable]
    class EventData<T>
    {
        public string type;
        public T payload;
        public string error = null;
    }
}
