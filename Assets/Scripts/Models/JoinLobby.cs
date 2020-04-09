using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class JoinLobby
    {
        public string lobby;

        public JoinLobby(string lobby)
        {
            this.lobby = lobby;
        }
    }
}
