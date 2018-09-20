using System.Collections.Generic;
using UnityEngine;

namespace LittleWorld
{
    [CreateAssetMenu(fileName = "EnvironmentData", menuName = "Environment", order = 1)]
    public class EnvironmentDatabase : ScriptableObject
    {
        public List<Environment> Environments;
    }
}