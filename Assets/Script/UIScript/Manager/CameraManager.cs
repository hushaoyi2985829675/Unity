using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CameraManager>();
                }
            }
            return _instance;
        }
    }

    private CinemachineVirtualCamera playerCam;
    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerCamScale(float orthoSize,float time,bool isOut = false,Action action = null)
    {
        StartCoroutine(PlayerCamAction(orthoSize, time, isOut,action));
    }

    private IEnumerator PlayerCamAction(float orthoSize,float time,bool isOut,Action action)
    {
        float curTime = 0;
        float startSize = playerCam.m_Lens.OrthographicSize;
        while (curTime < time)
        {
            float t;
            if (isOut)
            {
                t = Mathf.Pow(1 - (curTime / time), 3);                
            }
            else
            {
               t = 1 - Mathf.Pow(1 - (curTime / time), 3);                
            }
            
            float size = Mathf.Lerp(orthoSize,Math.Max(startSize, orthoSize),  t);
            playerCam.m_Lens.OrthographicSize = size;
            curTime += Time.deltaTime;
            yield return null;
        }
        playerCam.m_Lens.OrthographicSize = orthoSize;
        action?.Invoke();
    }

    public void ChangeMapAction(Action action)
    {
        PlayerCamScale(12.5f, 0.8f, true,() =>
        {
            // action?.Invoke();
            // PlayerCamScale(12.5f, 0.8f, true);
        });
    }
}
