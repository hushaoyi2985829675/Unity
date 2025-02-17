using HeroEditor.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class TalkHead : MonoBehaviour
{
    public SpriteCollection SpriteCollection;
    public GameObject playerHead;
    GameObject mouth;
    List<Sprite> sprList;
    bool isStop;
    Coroutine talkCoroutine;
    Coroutine headActionCoroutine;
    void Start()
    {
           
    }
    public void Enter()
    {
        sprList = new List<Sprite>();
        SpriteCollection = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/HeroEditor/Megapack/Resources/SpriteCollection.asset") as SpriteCollection;
        mouth = playerHead.transform.Find("Mouth").gameObject;
        sprList.Add(mouth.GetComponent<SpriteRenderer>().sprite);
        sprList.Add(SpriteCollection.Mouth.Find(info => info.Name == "WTF").Sprite);
        sprList.Add(SpriteCollection.Mouth.Find(info => info.Name == "XD").Sprite);
        DontDestroyOnLoad(gameObject);
    }
    IEnumerator ShowMouth()
    {
        while (true)
        {
            foreach (var sprite in sprList)
            {
                mouth.GetComponent<SpriteRenderer>().sprite = sprite;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    IEnumerator ShowHeadAction()
    {
        var angle = 5;
        var time = 0.3f;
        while (true)
        {
            for (int i = 0; i <= 5; i++)
            {
                playerHead.transform.localRotation = Quaternion.Euler(0, 0, i);
                yield return new WaitForSeconds(time / angle);
            }
            for (int i = 5; i >= 0; i--)
            {
                playerHead.transform.localRotation = Quaternion.Euler(0, 0, i);
                yield return new WaitForSeconds(time / angle);
            }
        }
    }
    public void StartTalk()
    {
        talkCoroutine = StartCoroutine(ShowMouth());
        headActionCoroutine = StartCoroutine(ShowHeadAction());
    }
    public void StopTalk()
    {
        StopCoroutine(talkCoroutine);
        StopCoroutine(headActionCoroutine);
        mouth.GetComponent<SpriteRenderer>().sprite = sprList[0];
        playerHead.transform.localRotation =  Quaternion.Euler(0, 0, 0);
    }
    void Update()
    {
        
    }
}
