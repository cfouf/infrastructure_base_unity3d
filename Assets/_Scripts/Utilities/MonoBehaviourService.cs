using System;
using UnityEngine;

namespace _Scripts.Utilities
{
    [DefaultExecutionOrder(-1)]
    public abstract class MonoBehaviourService<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        protected void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this as T;

            OnCreateService();
        }

        protected void OnDestroy()
        {
            if (instance == null) return;
            OnDestroyService();
            instance = null;
        }

        public static T Get(bool createIfDoesNotExist = false)
        {
            if (instance != null) return instance;
            if (!createIfDoesNotExist)
                throw new Exception($"Service {typeof(T).Name} not found");

            new GameObject($"Service {typeof(T).Name}").AddComponent<T>();
            Debug.LogWarning($"{typeof(T).Name} service was created");
            return instance;
        }


        protected abstract void OnCreateService();

        protected abstract void OnDestroyService();
    }
}