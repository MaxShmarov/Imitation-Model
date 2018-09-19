using System;
using UnityEngine;
using LittleWorld.Common;

namespace LittleWorld
{
    [Serializable]
    public class CurrentWeather
    {
        public int RainyIntensity;
        public int SunnyIntensity;
    }

    public class Cell : MonoBehaviour
    {

        private EnvironmentType _environmentType;
        [SerializeField]
        private CurrentWeather _currentWeather;

        private Weather _weather;

        private void Awake()
        {
            _weather = new Weather();
        }

        private void OnEnable()
        {
            EventManager.StartListening(Config.NextStep, UpdateWeatherVariable);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Config.NextStep, UpdateWeatherVariable);
        }

        private void Start()
        {
            SetRandomEnvironment();
            UpdateWeatherVariable();
        }

        private void SetRandomEnvironment()
        {
            Array values = Enum.GetValues(typeof(EnvironmentType));
            System.Random random = new System.Random();
            EnvironmentType randomEnvironment = (EnvironmentType)values.GetValue(random.Next(values.Length));
            _environmentType = randomEnvironment;
        }

        private void UpdateWeatherVariable()
        {
            _weather.UpdateWeather();
            _currentWeather.RainyIntensity = _weather.GetWeatherValueByType(WeatherType.Rainy);
            _currentWeather.SunnyIntensity = _weather.GetWeatherValueByType(WeatherType.Sunny);
            Debug.Log("Cell name : " + name + "; Cell type : " + _environmentType + "; rainy = " +
                _currentWeather.RainyIntensity + "; sunny = " + _currentWeather.SunnyIntensity);
        }
    }
}