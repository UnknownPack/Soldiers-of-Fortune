using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.backend
{
    public static class ListenerService 
    {
        private static Dictionary<string, Action> listeners = new Dictionary<string, Action>();

        public static void AddListener(string listenerId, Action callback)  
        {
            bool exists = listeners.ContainsKey(listenerId);
            listeners[listenerId] = exists? listeners[listenerId] + callback : callback;
        }
        
        public static void AddListener(string listenerId)
        {
            if (!listeners.ContainsKey(listenerId))
            {
                Debug.Log($"{listenerId} added to ListenerServocec");
                listeners[listenerId] = null;
            }
        }

        public static void RemoveListener(string listenerId, Action callback)
        {
            if (listeners.ContainsKey(listenerId))
            {
                listeners[listenerId] -= callback;

                if (listeners[listenerId] == null)
                    listeners.Remove(listenerId);
            }
        }

        public static void Notify(string listenerId)
        {
            if (listeners.TryGetValue(listenerId, out var action))
            {
                action?.Invoke(); 
                Debug.Log($"{listenerId} called!");
            }
            else
            {
                //Debug.LogWarning($"Listener {listenerId} not found!");
            }
        }

        public static void ClearAllListeners()
        {
            listeners.Clear();
            Debug.Log($"{listeners.Count} Listeners cleared!");
        }

    }
}
