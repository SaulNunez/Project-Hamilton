using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Puzzles.Variables.Int
{
    public class PuzzleStart : NetworkBehaviour
    {
        [SyncVar]
        private int defaultTemp;

        [SerializeField]
        TextMeshProUGUI currentTemp;

        [SerializeField]
        TextMeshProUGUI sliderTemp;

        public override void OnStartServer()
        {
            base.OnStartServer();

            defaultTemp = Random.Range(32, 99);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            currentTemp.text = $"{defaultTemp} C";
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
