using System;
using UnityEngine;
using LittleWorld.Common;
using LittleWorld.UI;

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
        private CellUI _cellUI;

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
            _cellUI.UpdateValues(_currentWeather.SunnyIntensity, _currentWeather.RainyIntensity);
        }
    }
}