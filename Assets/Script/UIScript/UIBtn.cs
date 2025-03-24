using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 pointerDownPosition;
    public GameObject Layer;
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        gameObject.transform.localScale =new Vector2(0.8f,0.8f);
        pointerDownPosition = eventData.position;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector2(1, 1);
        if (Vector2.Distance(pointerDownPosition, eventData.position) > 10f)
        {
            return;
        }
        UIManager.Instance.OpenLayer(Layer);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
