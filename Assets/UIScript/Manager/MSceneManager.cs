using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSceneManager : MonoBehaviour
{
    static MSceneManager _sceneManager;

    public MSceneManager instance
    {
        get
        {
            if (_sceneManager == null)
            {
                _sceneManager = new MSceneManager();
            }
            return _sceneManager;
        }
    }

    public static void LoadSceneMode()
    {
        
    }
}
