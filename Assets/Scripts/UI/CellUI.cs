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
        [SerializeField]
        private Text _grassJuiciness;
        [SerializeField]
        private Image _grassIcon;
        [SerializeField]
        private Text _rabbitCount;
        [SerializeField]
        private Image _rabbitIcon;
        [SerializeField]
        private Canvas _canvas;

        private void Start()
        {
            _canvas.worldCamera = Controllers.GameController.Instance._uiCamera;
        }

        public void UpdateUI(int sunIntensity, int rainIntensity, int grassJuiciness, int rabbitCount)
        {
            _sunIntensity.text = sunIntensity.ToString();
            _rainIntensity.text = rainIntensity.ToString();
            _grassJuiciness.text = grassJuiciness.ToString();
            _rabbitCount.text = rabbitCount.ToString();

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

            switch (grassJuiciness)
            {
                case 5:
                    _grassIcon.fillAmount = 1f;
                    break;
                case 4:
                    _grassIcon.fillAmount = 0.8f;
                    break;
                case 3:
                    _grassIcon.fillAmount = 0.6f;
                    break;
                case 2:
                    _grassIcon.fillAmount = 0.4f;
                    break;
                case 1:
                    _grassIcon.fillAmount = 0.2f;
                    break;
                default:
                    _grassIcon.fillAmount = 0f;
                    break;
            }

            switch (rabbitCount)
            {
                case 3:
                    _rabbitIcon.fillAmount = 1f;
                    break;
                case 2:
                    _rabbitIcon.fillAmount = 0.66f;
                    break;
                case 1:
                    _rabbitIcon.fillAmount = 0.33f;
                    break;
                default:
                    _rabbitIcon.fillAmount = 0f;
                    break;
            }
        }
    }
}