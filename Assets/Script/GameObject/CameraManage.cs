using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManage 
{
   private CameraManage instance;
   public CameraManage Instance
   {
      get
      {
         if (instance == null)
         {
            instance = new CameraManage();
            CinemachineBlenderSettings = AssetDatabase.LoadAssetAtPath<CinemachineBlenderSettings>("Assets/CameraBlends/CameraBlends.asset");
           // CinemachineBlendDefinition = CinemachineBlenderSettings.GetBlendForVirtualCameras(gameObject.name, ToCamera, new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.5f));
         }
         return instance;
      }
   }
   CinemachineBlenderSettings CinemachineBlenderSettings;
   CinemachineBlendDefinition CinemachineBlendDefinition;
   
}
