using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var typeObject = GameObject.Find("GameMain");
                if (typeObject != null)
                    instance = typeObject.GetComponent<T>();
            }
            return instance;
        }
    }
}
