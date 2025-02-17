using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tool 
{
    static Action PlayerEvent;

    public static RaycastHit2D Raycast(Vector2 pos, Vector2 direction, float distane,int layer,Vector2 playerPos)
    {
        var hit = Physics2D.Raycast(pos+ playerPos, direction, distane,layer);
        var color = hit? Color.green : Color.red;
        Debug.DrawRay(pos+playerPos, direction  *  distane,color);
        return hit;
    }
    public static Collider2D BoxCast(Vector2 pos, Vector2 size, int layer, Vector2 playerPos)
    {
        pos += playerPos;
        var hit = Physics2D.OverlapBox(pos, size, 0, layer);
        Vector2 halfSize = size / 2;
        Vector2[] boxCorners = new Vector2[4];
        boxCorners[0] = pos + new Vector2(-halfSize.x, -halfSize.y);  // ���½�
        boxCorners[1] = pos + new Vector2(halfSize.x, -halfSize.y);   // ���½�
        boxCorners[2] = pos + new Vector2(-halfSize.x, halfSize.y);   // ���Ͻ�
        boxCorners[3] = pos + new Vector2(halfSize.x, halfSize.y);    // ���Ͻ�
        Color color = hit ? Color.green : Color.red;
        Debug.DrawLine(boxCorners[0], boxCorners[1], color); // ���µ�����
        Debug.DrawLine(boxCorners[1], boxCorners[3], color); // ���µ�����
        Debug.DrawLine(boxCorners[3], boxCorners[2], color); // ���ϵ�����
        Debug.DrawLine(boxCorners[2], boxCorners[0], color); // ���ϵ�����
        return hit;
    }
    public static Collider2D OverlapCircle(Vector2 pos, float r, int layer)
    {
        var hit = Physics2D.OverlapCircle(pos, r, layer);
        return hit;
    }   
    public static void AddPlayerEvent(Action func)
    {
        PlayerEvent += func;
    }

    public static void onPlayerEvent()
    {
        PlayerEvent();
    }

    public static GameObject FindChlidTransform(GameObject gameObject, string name)
    {
        if (gameObject.transform.Find(name) != null)
        {
            return gameObject.transform.Find(name).gameObject;
        }
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var child = FindChlidTransform(gameObject.transform.GetChild(i).gameObject,name);
            if (child != null)
            {
                return child;
            }
        }
        return null;
    }
}
