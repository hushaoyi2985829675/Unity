using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    public Image maskLayer;
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
        float t;
        float size;
        float startSize = playerCam.m_Lens.OrthographicSize;
        while (curTime < time)
        {
            
            t = 1 - Mathf.Pow(1 - (curTime / time), 3);
            size = Mathf.Lerp(startSize,orthoSize,  t);
            playerCam.m_Lens.OrthographicSize = size;
            Color color = maskLayer.color;
            color.a = Mathf.Lerp(0,1, t);
            maskLayer.color = color;
            curTime += Time.deltaTime;
            yield return null;
        }
        playerCam.m_Lens.OrthographicSize = orthoSize;
        action?.Invoke();
        yield return new WaitForSeconds(0.5f);
        curTime = 0;
        while (curTime < time)
        {
            t = Mathf.Pow(curTime / time, 2);
            size = Mathf.Lerp(orthoSize,startSize,  t);
            playerCam.m_Lens.OrthographicSize = size;
            Color color = maskLayer.color;
            color.a = Mathf.Lerp(1,0, t);
            maskLayer.color = color;
            curTime += Time.deltaTime;
            yield return null;
        }
        playerCam.m_Lens.OrthographicSize = startSize;
        
    }

    public void ChangeMapAction(Action action)
    {
        PlayerCamScale(6f, 1f, true, () =>
        {
            action?.Invoke();
        });
    }
}
