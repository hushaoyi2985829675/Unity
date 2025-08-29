using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T) FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    go.AddComponent<T>();
                    _instance = go.GetComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }

            return _instance;
        }
    }
}