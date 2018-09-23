using UnityEngine;
using UnityEditor;
using LittleWorld.Common;
using System;

namespace LittleWorld
{
    public class Grass
    {
        private const int _minGrassJuiciness = 0;
        private const int _maxGrassJuiciness = 5;

        public int UpdateGrass(EnvironmentType environment, int sunIntesity, int rainIntensity, int grassJuiciness, bool waterBeside)
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
                newGrass = _minGrassJuiciness;
            }
            return newGrass;
        }

        public int Grow(int currentGrass)
        {
            if (currentGrass >= _maxGrassJuiciness)
            {
                return _maxGrassJuiciness;
            }
            currentGrass++;
            return currentGrass;
        }

        public int Wither(int currentGrass)
        {
            if (currentGrass <= _minGrassJuiciness)
            {
                return _minGrassJuiciness;
            }
            currentGrass--;
            return currentGrass;
        }
    }
}