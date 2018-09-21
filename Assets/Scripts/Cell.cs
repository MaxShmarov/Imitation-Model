using System;
using UnityEngine;
using LittleWorld.Common;
using UnityEngine.UI;

namespace LittleWorld
{
    public class CurrentWeather
    {
        public int RainyIntensity;
        public int SunnyIntensity;
    }

    public class Cell : MonoBehaviour
    {
        [SerializeField]
        private Renderer _renderer;
        [SerializeField]
        private Text _sunIntensity;
        [SerializeField]
        private Image _sunIcon;
        [SerializeField]
        private Text _rainIntensity;
        [SerializeField]
        private Image _rainIcon;

        private Vector3 _defaultCellSize = new Vector3(1f, 0.1f, 1f);
        private Environment _environment;
        private CurrentWeather _currentWeather;
        private Transform _transform;

        public Vector2Int _positionInMatrix { get; private set; }

        private void Awake()
        {
            _currentWeather = new CurrentWeather();
        }

        private void OnEnable()
        {
            EventManager.StartListening(Config.NextStep, UpdateWeatherVariable);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Config.NextStep, UpdateWeatherVariable);
        }

        public void Init(Vector3 position, string cellName,/* Environment environment,*/ Vector2Int index)
        {
            _transform = transform;
            _transform.localScale = _defaultCellSize;
            _transform.position = position;
            name = cellName;
            _environment = Database.Instance.GetRandomEnvironment();
            _positionInMatrix = index;
            EnvironmentFromData();
            UpdateWeatherVariable();          
        }

        private void EnvironmentFromData()
        {
            if (_environment == null || _renderer == null)
                return;
            _renderer.material.color = _environment.Color;
        }

        private void UpdateWeatherVariable()
        {
            _currentWeather = Database.Instance.Weather.GetRandomWeather();
            _sunIntensity.text = _currentWeather.SunnyIntensity.ToString();
            _rainIntensity.text = _currentWeather.RainyIntensity.ToString();
            switch (_currentWeather.SunnyIntensity)
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

            switch (_currentWeather.RainyIntensity)
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

            //    Debug.Log("Cell name : " + name + "; Cell type : " + _environment.Type + "; rainy = " +
            //        _currentWeather.RainyIntensity + "; sunny = " + _currentWeather.SunnyIntensity);
        }
    }
}