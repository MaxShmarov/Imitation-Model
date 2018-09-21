using System;
using UnityEngine;

namespace LittleWorld
{
    [Serializable]
    public class Environment
    {
        public string Name;
        public EnvironmentType Type;
        public Color Color;
        public bool GrassMayGrow;
    }
}