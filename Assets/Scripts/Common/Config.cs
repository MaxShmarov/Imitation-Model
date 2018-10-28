using UnityEngine;

namespace LittleWorld.Common
{
    public static class Config
    {
        public const string NextStep = "NextStep";

        public static int SizeX;
        public static int SizeY;

        public const int MinCellCount = 1;
        public const int MaxCellCount = 100;

        public const int MinWeatherIntensity = 0;
        public const int MaxWeatherIntensity = 3;

        public const int MinGrassJuiciness = 0;
        public const int MaxGrassJuiciness = 5;

        public const int MinRabbitCount = 0;
        public const int MaxRabbitCount = 3;

        public static int GetRandomValue(int min, int max)
        {
            return Random.Range(min, max);
        }

        public static CurrentWeather GetRandomWeather()
        {
            CurrentWeather newWeather = new CurrentWeather();
            int rainyIntensity, sunnyIntensity = MinWeatherIntensity;

            rainyIntensity = GetRandomValue(MinWeatherIntensity, MaxWeatherIntensity + 1);
            if (rainyIntensity == MaxWeatherIntensity)
            {
                sunnyIntensity = MinWeatherIntensity;
            }
            else
            {
                sunnyIntensity = GetRandomValue(MinWeatherIntensity, MaxWeatherIntensity + 1);
                if (sunnyIntensity == MaxWeatherIntensity)
                {
                    rainyIntensity = MinWeatherIntensity;
                }
            }
            newWeather.RainyIntensity = rainyIntensity;
            newWeather.SunnyIntensity = sunnyIntensity;
            return newWeather;
        }

        public static int UpdateGrass(EnvironmentType environment, int sunIntesity, int rainIntensity, int grassJuiciness, bool waterBeside)
        {
            var newGrass = grassJuiciness;
            if (environment == EnvironmentType.Field)
            {
                if (rainIntensity == 0 && sunIntesity == 0)
                    newGrass = grassJuiciness;
                else if (rainIntensity == 1 && sunIntesity == 0)
                    newGrass = grassJuiciness;
                else if (rainIntensity == 2 && sunIntesity == 0)
                    newGrass = grassJuiciness;
                else if (rainIntensity == 3 && sunIntesity == 0)
                    newGrass = Wither(grassJuiciness);
                else if (rainIntensity == 0 && sunIntesity == 1)
                {
                    if (waterBeside)
                    {
                        newGrass = Grow(grassJuiciness);
                    }
                    else
                    {
                        newGrass = grassJuiciness;
                    }
                }
                else if (rainIntensity == 1 && sunIntesity == 1)
                    newGrass = Grow(grassJuiciness);
                else if (rainIntensity == 2 && sunIntesity == 1)
                    newGrass = Grow(grassJuiciness);
                else if (rainIntensity == 0 && sunIntesity == 2)
                {
                    if (waterBeside)
                    {
                        newGrass = Grow(grassJuiciness);
                    }
                    else
                    {
                        newGrass = grassJuiciness;
                    }
                }
                else if (rainIntensity == 1 && sunIntesity == 2)
                    newGrass = Grow(grassJuiciness);
                else if (rainIntensity == 2 && sunIntesity == 2)
                    newGrass = Grow(grassJuiciness);
                else if (rainIntensity == 0 && sunIntesity == 3)
                {
                    if (waterBeside)
                    {
                        newGrass = Grow(grassJuiciness);
                    }
                    else
                    {
                        newGrass = Wither(grassJuiciness);
                    }
                }
                else
                    newGrass = grassJuiciness;
            }
            else
            {
                newGrass = MinGrassJuiciness;
            }
            return newGrass;
        }

        private static int Grow(int currentGrass)
        {
            if (currentGrass >= MaxGrassJuiciness)
            {
                return MaxGrassJuiciness;
            }
            currentGrass++;
            return currentGrass;
        }

        private static int Wither(int currentGrass)
        {
            if (currentGrass <= MinGrassJuiciness)
            {
                return MinGrassJuiciness;
            }
            currentGrass--;
            return currentGrass;
        }

        public static int UpdateRabbits(int currentGrass, int rabbitCount)
        {
           if(currentGrass < rabbitCount)
            {

            }
            return 0;
        }


        private static int AddRabbit(int rabbitCount)
        {
            if (rabbitCount >= MaxRabbitCount)
            {
                return MaxGrassJuiciness;
            }
            rabbitCount++;
            return rabbitCount;
        }

        private static int RemoveRabbit(int rabbitCount)
        {
            if (rabbitCount <= MinCellCount)
            {
                return MinGrassJuiciness;
            }
            rabbitCount--;
            return rabbitCount;
        }
    }
}