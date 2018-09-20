using UnityEngine;

namespace LittleWorld.Common
{
    public static class Config
    {
        public const string NextStep = "NextStep";

        public static int GetRandomValue(int min, int max)
        {
            return Random.Range(min, max + 1);
        }
    }
}