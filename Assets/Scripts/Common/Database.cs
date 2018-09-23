using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace LittleWorld.Common
{
    public class Database : Singleton<Database>
    {
        [SerializeField]
        private EnvironmentDatabase EnvironmentDatabase;
        private Weather _weather = new Weather();
        public Weather Weather
        {
            get
            {
                return _weather;
            }
        }

        private Grass _grass = new Grass();
        public Grass Grass
        {
            get
            {
                return _grass;

            }
        }

        public Environment GetRandomEnvironment()
        {
            var randomIndex = Config.GetRandomValue(0, EnvironmentDatabase.Environments.Count);
            return EnvironmentDatabase.Environments[randomIndex];
        }

        public List<Environment> GetEnvironmentsData()
        {
            return EnvironmentDatabase.Environments;
        }

        public Environment GetEnvironmentsDataByType(EnvironmentType type)
        {
            Environment data = EnvironmentDatabase.Environments.Find(x => x.Type == type);
            return data;
        }
    }
}