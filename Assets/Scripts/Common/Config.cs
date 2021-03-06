﻿using System;
using UnityEngine;

namespace LittleWorld.Common
{
    public static class Config
    {
        public const string NextStep = "NextStep";

        public static int SizeX;
        public static int SizeY;

        public static int? FieldCount;
        public static int? MountainCount;
        public static int? LakeCount;

        public const int MinCellCount = 1;
        public const int MaxCellCount = 100;

        public const int MinWeatherIntensity = 0;
        public const int MaxWeatherIntensity = 3;

        public const int MinGrassJuiciness = 0;
        public const int MaxGrassJuiciness = 5;

        public const int MinThingCount = 0;
        public const int MaxThingCount = 3;

        public static int AllRabbits = 0;
        public static int AllWolves = 0;
        public static int AllHunters = 0;

        public static int SetRabbits = 0;
        public static int SetWolves = 0;
        public static int SetHunters = 0;

        public static int GetRandomValue(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static void ClearLifeState()
        {
            AllRabbits = 0;
            AllWolves = 0;
            AllHunters = 0;
            ClearAddedCreatures();
        }

        public static void ClearAddedCreatures()
        {
            SetRabbits = 0;
            SetWolves = 0;
            SetHunters = 0;
        }

        public static void CalculateEnvironment(int percentField, int percentMountain, int percentLake)
        {
            int sum = percentField + percentMountain + percentLake;
            if (sum < 100 || sum > 100)
            {
                Debug.LogError("Percents summ incorrect. It must be 100%");
                return;
            }
            int matrixSize = SizeX * SizeY;
            FieldCount = Mathf.RoundToInt(matrixSize * (percentField / 100f));
            MountainCount = Mathf.RoundToInt(matrixSize * (percentMountain / 100f));
            LakeCount = Mathf.RoundToInt(matrixSize * (percentLake / 100f));
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

            if (currentGrass < MinGrassJuiciness)
            {
                return MinGrassJuiciness;
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

            if (currentGrass > MaxGrassJuiciness)
            {
                return MaxGrassJuiciness;
            }

            currentGrass--;
            return currentGrass;
        }

        public static int UpdateRabbits(int currentGrass, int rabbitCount)
        {
            //Code: -1 - too many rabbits(leave or die)
            if (currentGrass < rabbitCount)
            {
                return -1;
            }
            else if (rabbitCount >= 2)
            {
                return AddThing(rabbitCount);
            }
            return rabbitCount;
        }


        public static int AddThing(int count)
        {
            if (count >= MaxThingCount)
            {
                return MaxThingCount;
            }
            if (count <= MinThingCount)
            {
                return 1;
            }
            count++;
            return count;
        }

        public static int RemoveThing(int count)
        {
            if (count <= MinThingCount)
            {
                return MinThingCount;
            }

            if (count > MaxThingCount)
            {
                return MaxThingCount;
            }
            count--;
            return count;
        }
    }
}