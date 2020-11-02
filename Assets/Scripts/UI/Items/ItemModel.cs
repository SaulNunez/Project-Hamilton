using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Items
{
    public class ItemModel : MonoBehaviour
    {
        public Text nameLabel;
        public Text descriptionLabel;
        public Image image;

        public string Name { get => nameLabel.text; set => nameLabel.text = value; }
        public string Description { get => descriptionLabel.text; set => descriptionLabel.text = value; }
        public Sprite ItemSprite { get => image.sprite; set => image.sprite = value; }
    }
}
