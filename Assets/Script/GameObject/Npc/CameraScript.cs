using Cinemachine;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    CinemachineBlenderSettings CinemachineBlenderSettings;
    CinemachineBlendDefinition CinemachineBlendDefinition;
    public string ToCamera;
    void Start()
    {
        //CinemachineBlenderSettings = AssetDatabase.LoadAssetAtPath<CinemachineBlenderSettings>("Assets/Editor/CameraBlends/CameraBlends.asset");
      //  CinemachineBlendDefinition = CinemachineBlenderSettings.GetBlendForVirtualCameras(gameObject.name, ToCamera, new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.5f));
    }

    public void HideCamera(Action callback)
    {
        GetComponent<CinemachineVirtualCamera>().Priority = 0;
        StartCoroutine(DelayCallback(callback));
    }
    IEnumerator DelayCallback(Action callback)
    {
        yield return new WaitForSeconds(CinemachineBlendDefinition.m_Time);
        callback();
    }
    public void SetFollowTarget(Transform transform)
    {
        GetComponent<CinemachineVirtualCamera>().Follow = transform;
    }
}
