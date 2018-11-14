using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System;

namespace LittleWorld.Data
{
    [CreateAssetMenu(fileName = "New data", menuName = "Data", order = 1)]
    public class WorldData : ScriptableObject
    {
        [SerializeField]
        private List<Environment> _environmentsData;
        [SerializeField]
        private List<Weather> _weatherData;
        [SerializeField]
        private Grass _grassData;
        [SerializeField]
        private Rabbit _rabbitData;

        public Environment GetRandomEnvironment()
        {
            return _environmentsData.RandomItem();
        }

        public Environment GetEnvironmentByType(EnvironmentType type)
        {
            return _environmentsData.Find(x => x.Type == type);
        }

        public List<Environment> GetEnvironments()
        {
            return _environmentsData;
        }

        public List<Weather> GetWeathers()
        {
            return _weatherData;
        }

        public Grass GetGrass()
        {
            return _grassData;
        }

        public Rabbit GetRabbits()
        {
            return _rabbitData;
        }
    }
}