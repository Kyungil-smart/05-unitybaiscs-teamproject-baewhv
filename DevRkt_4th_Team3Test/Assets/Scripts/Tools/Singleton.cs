using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T>: MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }

    protected void SingletonInit()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T; //업캐스팅
            DontDestroyOnLoad(_instance.gameObject);
        }
    }
}
