using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Src.Backend
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new();

        public static void Register<T>(T script)
        {
            var scriptType = typeof(T);
            if (Services.ContainsKey(scriptType))
                Debug.LogWarning($"Replacing script of type: {typeof(T)}!");
            
            Services[scriptType] = script;
        }

        public static T Get<T>()
        {
            var scriptType = typeof(T);
            if (Services.ContainsKey(scriptType))
                return (T)Services[scriptType];
            
            Debug.LogError($"Could not find {typeof(T)} you were looking for! Returning null.");
            return default;
        }
        
        public static bool HasService<T>()
        {
            return Services.ContainsKey(typeof(T));
        }

        
        public static async Task<T> WaitForServiceAsync<T>() where T : class
        {
            while (!HasService<T>())
            {
                await Task.Delay(100); 
            }

            //Debug.Log($"[ServiceLocator] {typeof(T).Name} is now registered!");
            return Get<T>();
        }
        
        public static async Task WaitForServiceAsync<T>(Action<T> callback) where T : class
        {
            var service = await WaitForServiceAsync<T>();
            callback?.Invoke(service);
        }

        public static void Unregister<T>()
        {
            var scriptType = typeof(T);
            if (Services.ContainsKey(scriptType))
                Services.Remove(scriptType);
        }

        public static void ClearServices() => Services.Clear();
    }
}