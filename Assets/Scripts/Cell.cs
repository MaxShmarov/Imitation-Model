using System;
using UnityEngine;
using LittleWorld.Common;

namespace LittleWorld
{
    public class CurrentWeather
    {
        public int RainyIntensity;
        public int SunnyIntensity;
    }

    public class Cell : MonoBehaviour
    {
        private Environment _environment;
        private Renderer _renderer;
        private CurrentWeather _currentWeather;
        private Transform _transform;
        private Weather _weather;
        public Vector2Int _positionInMatrix { get; private set; }
        private Vector3 _defaultCellSize = new Vector3(1f, 0.1f, 1f);

        private void Awake()
        {
            _currentWeather = new CurrentWeather();
            _weather = new Weather();
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            EventManager.StartListening(Config.NextStep, UpdateWeatherVariable);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Config.NextStep, UpdateWeatherVariable);
        }

        public void Init(Vector3 position, string cellName, Environment environment, Vector2Int index)
        {
            _transform = transform;
            _transform.localScale = _defaultCellSize;
            _transform.position = position;
            name = cellName;
            _environment = environment;
            _positionInMatrix = index;
            EnvironmentFromData();
            UpdateWeatherVariable();          
        }

        private void EnvironmentFromData()
        {
            if (_environment == null || _renderer == null)
                return;
            _renderer.material = _environment.EnvironmentMaterial;
            _renderer.material.color = _environment.Color;
        }

        private void UpdateWeatherVariable()
        {
            _weather.UpdateWeather();
            _currentWeather.RainyIntensity = _weather.GetWeatherValueByType(WeatherType.Rainy);
            _currentWeather.SunnyIntensity = _weather.GetWeatherValueByType(WeatherType.Sunny);
            Debug.Log("Cell name : " + name + "; Cell type : " + _environment.Type + "; rainy = " +
                _currentWeather.RainyIntensity + "; sunny = " + _currentWeather.SunnyIntensity);
        }
    }
}