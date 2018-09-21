using System.Collections.Generic;
using LittleWorld.Common;

namespace LittleWorld
{
    public class Weather
    {
        private const int _minWeatherIntensity = 0;
        private const int _maxWeatherIntensity = 3;

        public CurrentWeather GetRandomWeather()
        {
            CurrentWeather newWeather = new CurrentWeather();
            int rainyIntensity, sunnyIntensity = _minWeatherIntensity;

            rainyIntensity = Config.GetRandomValue(_minWeatherIntensity, _maxWeatherIntensity, true);
            if (rainyIntensity == _maxWeatherIntensity)
            {
                sunnyIntensity = _minWeatherIntensity;                
            }
            else
            {
                sunnyIntensity = Config.GetRandomValue(_minWeatherIntensity, _maxWeatherIntensity, true);
                if (sunnyIntensity == _maxWeatherIntensity)
                {
                    rainyIntensity = _minWeatherIntensity;                   
                }
            }
            newWeather.RainyIntensity = rainyIntensity;
            newWeather.SunnyIntensity = sunnyIntensity;
            return newWeather;
        }
    }
}