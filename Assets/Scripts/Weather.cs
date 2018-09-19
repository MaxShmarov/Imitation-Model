using System.Collections.Generic;
using UnityEngine;

namespace LittleWorld
{
    public class Weather
    {
        private const int _minWeatherIntensity = 0;
        private const int _maxWeatherIntensity = 3;

        private WeatherType _rainy = WeatherType.Rainy;
        private WeatherType _sunny = WeatherType.Sunny;

        private Dictionary<WeatherType, int> _weather = new Dictionary<WeatherType, int>();

        public int GetWeatherValueByType(WeatherType type)
        {
            return _weather[type];
        }

        public void UpdateWeather()
        {
            if (_weather.ContainsKey(_rainy))
            {
                var randomValue = GetRandomValue();
                _weather[_rainy] = randomValue;
                if (randomValue == _maxWeatherIntensity)
                {
                    _weather[_sunny] = _minWeatherIntensity;
                    return;
                }
            }
            else
            {
                AddWeatherToDictionary(_rainy);
            }
                

            if (_weather.ContainsKey(WeatherType.Sunny))
            {
                var randomValue = GetRandomValue();
                _weather[_sunny] = randomValue;
                if (randomValue == _maxWeatherIntensity)
                {
                    _weather[_rainy] = _minWeatherIntensity;
                    return;
                }
            }              
            else
            {
                AddWeatherToDictionary(_sunny);
            }                
        }

        private int GetRandomValue()
        {
            return Random.Range(_minWeatherIntensity, _maxWeatherIntensity + 1);
        }

        private void AddWeatherToDictionary(WeatherType type)
        {
            _weather.Add(type, _minWeatherIntensity);
        }
    }
}