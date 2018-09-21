using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LittleWorld.UI
{
    public class CellUI : MonoBehaviour
    {
        [SerializeField]
        private Text _sunIntensity;
        [SerializeField]
        private Image _sunIcon;
        [SerializeField]
        private Text _rainIntensity;
        [SerializeField]
        private Image _rainIcon;

        public void UpdateValues(int sunIntensity, int rainIntensity)
        {
            _sunIntensity.text = sunIntensity.ToString();
            _rainIntensity.text = rainIntensity.ToString();

            switch (sunIntensity)
            {
                case 3:
                    _sunIcon.fillAmount = 1f;
                    break;
                case 2:
                    _sunIcon.fillAmount = 0.66f;
                    break;
                case 1:
                    _sunIcon.fillAmount = 0.33f;
                    break;
                default:
                    _sunIcon.fillAmount = 0f;
                    break;
            }

            switch (rainIntensity)
            {
                case 3:
                    _rainIcon.fillAmount = 1f;
                    break;
                case 2:
                    _rainIcon.fillAmount = 0.66f;
                    break;
                case 1:
                    _rainIcon.fillAmount = 0.33f;
                    break;
                default:
                    _rainIcon.fillAmount = 0f;
                    break;
            }
        }
    }
}