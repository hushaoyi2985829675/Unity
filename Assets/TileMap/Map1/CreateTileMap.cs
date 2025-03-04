using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateTileMap : MonoBehaviour
{
    [Header("地面Tilemap")]
    public Tilemap ground;
    [Header("地面RuleTile")]
    public TileBase tile;
    [Header("地面起始点")]
    public Vector2Int startPosition = new Vector2Int(-15, -5);
    [Header("地面生长范围")]
    public Vector2Int groundLenRan = new Vector2Int(2,6);
    [Header("地面随机范围")]
    public Vector2 groundRange = new Vector2(0.7f, 0.9f);
    [Header("地图高度")]
    public int height = 20;
    [Header("地图宽度")]
    public int width = 100;
    void Start()
    {
        StartCoroutine(CreateGround());
    }

    public IEnumerator CreateGround()
    {
        for (int i = startPosition.x; i < startPosition.x + width; i++)
        {
            for (int j = startPosition.y; j < startPosition.x + height; j++)
            {
                bool isGround = j <= startPosition.y + 3 ? Random.value > groundRange.x : Random.value > groundRange.y;
                if (isGround)
                {
                    yield return StartCoroutine(GroundExtension(i,j));
                }
            }
            
        }

        StartCoroutine(ClearGround());
    }

    public IEnumerator GroundExtension(int m,int n)
    {
        int w = Random.Range(groundLenRan.x, groundLenRan.y);
        int h = Random.Range(1,4);
        for (int i = m; i < m + w; i++)
        {
            for (int j = n; j < n + h; j++)
            {
              
                ground.SetTile(new Vector3Int(i,j,0),tile);
            }
        }
        yield return null;
    }
    //清理出人能通过的通道
    public IEnumerator ClearGround()
    {
        for (int i = startPosition.x; i < startPosition.x + width; i++)
        {
            for (int j = startPosition.y; j < startPosition.x + height; j++)
            {
                if (ground.GetTile(new Vector3Int(i, j, 0)) != null)
                {
                    if (ground.GetTile(new Vector3Int(i, j + 1, 0)) == null)
                    {
                        yield return null;
                        ground.SetTilesBlock(new BoundsInt(i-2,j+1,0,3,3,1),new TileBase[] {null,null, null, null, null, null,null, null, null});
                    }
                }
            }
           
        }
    }
}
