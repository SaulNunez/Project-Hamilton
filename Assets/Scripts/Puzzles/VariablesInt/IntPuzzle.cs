using Mirror;
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace Puzzles.Variables.Int
{
    public class IntPuzzle : NetworkBehaviour
    {
        [SyncVar]
        public int defTem;

        [SyncVar]
        public int sliderDefTem;

        [SerializeField]
        TextMeshProUGUI currentTemp;

        [SerializeField]
        TextMeshProUGUI sliderTemp;

        [SerializeField]
        Slider tempSlider;

        public override void OnStartServer()
        {
            base.OnStartServer();

            defTem = Random.Range(32, 99);

            //Suma o resta una cantidad aleatoria a defTemp como valor inicial del slider 
            sliderDefTem = (int)Mathf.Clamp(defTem + (Random.Range(5, 20) * Mathf.Sign(Random.Range(-1, 1))), 32f, 99f);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            currentTemp.text = $"{defTem} C";
            sliderTemp.text = $"{sliderDefTem} C";
            tempSlider.value = sliderDefTem;
        }

        private void OnEnable()
        {
            if (isClient)
            {
                currentTemp.text = $"{defTem} C";
                tempSlider.value = sliderDefTem;
            }
        }

        public void OnInputUpdate(float value)
        {
            sliderDefTem = (int)value;
            sliderTemp.text = $"{sliderDefTem} C";

            if(value == defTem)
            {
                PuzzleCompletion.instance.MarkCompleted(PuzzleId.BoilersVariableInteger);
            }
        }
    }
}
