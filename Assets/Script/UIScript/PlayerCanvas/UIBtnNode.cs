using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnNode : MonoBehaviour
{
    [SerializeField] List<GameObject> btnList = new List<GameObject>();
    [SerializeField] Button btn;
    [SerializeField] float aniTime = 0.1f;
    private bool isOpen;

    void Start()
    {
        isOpen = false;
        btn.onClick.AddListener(OnShowUIClick);
        for (int i = 0; i < btnList.Count; i++)
        {
            GameObject obj = btnList[i];
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnShowUIClick()
    {
        btn.transform.DORotate(new Vector3(0, 0, -45 - 180), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutExpo);
        if (isOpen)
        {
            float time = 0;
            for (int i = btnList.Count - 1; i >= 0; i--)
            {
                GameObject obj = btnList[i];
                DOVirtual.DelayedCall(time,
                    () => { obj.SetActive(false); });
                time += aniTime;
            }

            isOpen = false;
        }
        else
        {
            float time = 0;
            for (int i = 0; i < btnList.Count; i++)
            {
                GameObject obj = btnList[i];
                DOVirtual.DelayedCall(time,
                    () => { obj.SetActive(true); });
                time += aniTime;
            }

            isOpen = true;
        }
    }
}