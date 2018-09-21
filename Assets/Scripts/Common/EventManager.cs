using System;
using System.Collections.Generic;

namespace LittleWorld.Common
{
    public static class EventManager
    {
        private static Dictionary<string, Action> _eventTable = new Dictionary<string, Action>();

        public static void StartListening(string eventName, Action handler)
        {
            lock (_eventTable)
            {
                if (!_eventTable.ContainsKey(eventName))
                {
                    _eventTable.Add(eventName, null);
                }
                _eventTable[eventName] += handler;
            }
        }

        public static void StopListening(string eventName, Action handler)
        {
            lock (_eventTable)
            {
                if (_eventTable.ContainsKey(eventName))
                {
                    _eventTable[eventName] -= handler;
                    if (_eventTable[eventName] == null)
                    {
                        _eventTable.Remove(eventName);
                    }
                }
            }
        }

        public static void Trigger(string eventName)
        {
            Action a;
            if (_eventTable.TryGetValue(eventName, out a))
            {
                Action callback = a;
                if (callback != null)
                {
                    callback();
                }
            }
        }
    }

    public static class EventManager<T>
    {
        private static Dictionary<string, Action<T>> _eventTable = new Dictionary<string, Action<T>>();

        public static void StartListening(string eventName, Action<T> handler)
        {
            lock (_eventTable)
            {
                if (!_eventTable.ContainsKey(eventName))
                {
                    _eventTable.Add(eventName, null);
                }
                _eventTable[eventName] += handler;
            }
        }

        public static void StopListening(string eventName, Action<T> handler)
        {
            lock (_eventTable)
            {
                if (_eventTable.ContainsKey(eventName))
                {
                    _eventTable[eventName] -= handler;
                    if (_eventTable[eventName] == null)
                    {
                        _eventTable.Remove(eventName);
                    }
                }
            }
        }

        public static void Trigger(string eventName, T arg)
        {
            Action<T> a;
            if (_eventTable.TryGetValue(eventName, out a))
            {
                Action<T> callback = a;
                if (callback != null)
                {
                    callback(arg);
                }
            }
        }
    }

    public static class EventManager<T, U>
    {
        private static Dictionary<string, Action<T, U>> _eventTable = new Dictionary<string, Action<T, U>>();

        public static void StartListening(string eventName, Action<T, U> handler)
        {
            lock (_eventTable)
            {
                if (!_eventTable.ContainsKey(eventName))
                {
                    _eventTable.Add(eventName, null);
                }
                _eventTable[eventName] += handler;
            }
        }

        public static void StopListening(string eventName, Action<T, U> handler)
        {
            lock (_eventTable)
            {
                if (_eventTable.ContainsKey(eventName))
                {
                    _eventTable[eventName] -= handler;
                    if (_eventTable[eventName] == null)
                    {
                        _eventTable.Remove(eventName);
                    }
                }
            }
        }

        public static void Trigger(string eventName, T arg0, U arg1)
        {
            Action<T, U> a;
            if (_eventTable.TryGetValue(eventName, out a))
            {
                Action<T, U> callback = a;
                if (callback != null)
                {
                    callback(arg0, arg1);
                }
            }
        }
    }
}