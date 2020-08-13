using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

namespace Assets.Scripts.Websockets
{
    public static class StringWebSocketSend
    {
        public static void SendText(this WebSocket wb, string message)
        {
            wb.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}
