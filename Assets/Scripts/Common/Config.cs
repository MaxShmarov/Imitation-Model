using UnityEngine;

namespace LittleWorld.Common
{
    public static class Config
    {
        public const string NextStep = "NextStep";
        public const int MinCellCount = 1;
        public const int MaxCellCount = 100;

        public static int GetRandomValue(int min, int max, bool inclusiveMax = false)
        {
            if (inclusiveMax)
                return Random.Range(min, max + 1);
            else
                return Random.Range(min, max);
        }
    }
}