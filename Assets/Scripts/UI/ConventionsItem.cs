using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LittleWorld.UI
{
    public class ConventionsItem : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private Text _name;

        public void Init(Color color, string name)
        {
            _icon.color = color;
            _name.text = name;
        }
    }
}