using UnityEngine;

namespace Helpers
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T _instance;

        public void Awake()
        {
            if (!_instance)
            {
                _instance = gameObject.GetComponent<T>();
            }
            else
            {
                Debug.LogError("<color=red>[Singleton] Second instance of '" + typeof(T) + "' created!</color>");
            }
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("<color=red>[Singleton] multiple instances of '" + typeof(T) + "' found!</color>");
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "[Singleton] " + typeof(T).ToString();
                        DontDestroyOnLoad(singleton);
                        Debug.Log("[Singleton] An instance of '" + typeof(T) + "' was created: " + singleton);
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance of '" + typeof(T) + "': " + _instance.gameObject.name);
                    }
                }
                return _instance;
            }
        }
    }
}