using Mirror;
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace Puzzles.Variables.Int
{
    /// <summary>
    /// Handles the logic of both float and int thermostat puzzle variants
    /// </summary>
    public class IntPuzzle : NetworkBehaviour
    {
        [SyncVar]
        float defTem;

        [SyncVar]
        float sliderDefTem;

        [SerializeField]
        bool useFloatDefaultTemperature = false;

        [SerializeField]
        TextMeshProUGUI currentTemp;

        [SerializeField]
        TextMeshProUGUI sliderTemp;

        [SerializeField]
        Slider tempSlider;

        public override void OnStartServer()
        {
            base.OnStartServer();

            defTem = useFloatDefaultTemperature? (float)Math.Round(Random.Range(32f, 99f), 2) : Random.Range(32, 99);

            //Suma o resta una cantidad aleatoria a defTemp como valor inicial del slider 
            sliderDefTem = Mathf.Clamp(defTem + (Random.Range(5, 20) * Mathf.Sign(Random.Range(-1, 1))), 32f, 99f);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            currentTemp.text = $"{defTem} C";
            if(sliderTemp != null)
            {
                sliderTemp.text = $"{sliderDefTem} C";
            }
            if(tempSlider != null)
            {
                tempSlider.value = sliderDefTem;
            }
        }

        private void OnEnable()
        {
            if (isClient)
            {
                currentTemp.text = $"{defTem} C";
                if (tempSlider != null)
                {
                    tempSlider.value = sliderDefTem;
                }
            }
        }

        /// <summary>
        /// Exposed to be linked on editor to on value changed on slider
        /// </summary>
        /// <param name="value"></param>
        public void OnInputUpdate(float value)
        {
            sliderDefTem = value;
            if (sliderTemp != null)
            {
                sliderTemp.text = $"{sliderDefTem} C";
            }
            if (value == defTem)
            {
                CmdCheckInput(value);
            }
        }

        /// <summary>
        /// Exposed to be linked on editor to onEndEdit of a InputField
        /// </summary>
        /// <param name="value">InputField content</param>
        public void OnInputUpdate(string value)
        {
            try
            {
                var capturedValue = float.Parse(value);

                if (capturedValue == defTem)
                {
                    CmdCheckInput(capturedValue);
                }
            }catch(Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// Check input on server
        /// </summary>
        /// <param name="value"></param>
        [Command]
        void CmdCheckInput(float value)
        {
            if (value == defTem)
            {
                PuzzleCompletion.instance.MarkCompleted(useFloatDefaultTemperature ? PuzzleId.VariableFloat : PuzzleId.BoilersVariableInteger);
            }
        }
    }
}
