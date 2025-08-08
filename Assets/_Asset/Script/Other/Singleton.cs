using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //Built-in singleton
    private static T instance;
    private static object _lock = new object();
    private static bool applicationIssQuitting = false;
    public static T Instance
    {
        get 
        { 
            if (applicationIssQuitting)
            {
                return null;
            }

            lock (_lock)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject singletonObj = new GameObject($"{typeof(T)} (Singleton)");
                        instance = singletonObj.AddComponent<T>();
                    }
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }
    
    protected virtual void OnDestroy()
    {
        applicationIssQuitting = true;
    }    
}
