using System;
using UnityEngine;

namespace bdeshi.utility
{
    public class MonoBehaviourLazySingleton<T> : MonoBehaviour
        where T : MonoBehaviourLazySingleton<T> 
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {

                    if (appicationQuitting)
                        return null;

                    _instance = FindObjectOfType<T>();
                    // Debug.Log("found " + _instance);
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).ToString());
                        // Debug.Log(obj + "create");
                        //obj.hideFlags = HideFlags.HideAndDontSave;
                        _instance = obj.AddComponent<T>();
                        _instance.initialize();
                    }
                }
                // Debug.Log("end "+ (_instance == null));

                return _instance;
            }
        }


        private static bool appicationQuitting;

        protected virtual void initialize() { }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if(_instance != this)
            {
                Destroy(gameObject);
            }
        }


        private void OnApplicationQuit()
        {
            appicationQuitting = true;
        }

        public static void PlayModeEnterCleanup()
        {
            appicationQuitting = false;
        }
        

    }
}
