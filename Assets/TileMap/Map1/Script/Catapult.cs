using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    private RaycastHit2D leftHit;
    private RaycastHit2D rightHit;
    private RaycastHit2D hit;
    private bool isJump = false;

    [SerializeField]
    private float jumpValue = 22;

    void Start()
    {
        // transform.DOScaleY(0.6f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        // {
        //     transform.DOScaleY(1f, 0.3f).SetEase(Ease.OutBack, 3f).SetLoops(1, LoopType.Restart);
        // });
    }

    // Update is called once per frame
    void Update()
    {
        leftHit = Tool.Raycast(new Vector2(-0.5f, 0.9f), Vector2.up, 0.4f, LayerMask.GetMask("Player"), transform.position);
        rightHit = Tool.Raycast(new Vector2(0.5f, 0.9f), Vector2.up, 0.4f, LayerMask.GetMask("Player"), transform.position);
        if (leftHit)
        {
            hit = leftHit;
        }
        else if (rightHit)
        {
            hit = rightHit;
        }
        else
        {
            hit = new RaycastHit2D();
        }

        if (hit && !isJump)
        {
            isJump = true;
            transform.DOScaleY(0.6f, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                AudioManager.Instance.PlayAudio(gameObject, AudioType.Catapult, "Pop");
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    if (hit)
                    {
                        Player player = hit.collider.gameObject.GetComponent<Player>();
                        Rigidbody2D rd = player.GetComponent<Rigidbody2D>();
                        rd.AddForce(new Vector2(0, jumpValue), ForceMode2D.Impulse);
                    }

                    transform.DOScaleY(1.5f, 0.15f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                        transform.DOScaleY(1f, 0.3f).SetEase(Ease.OutBack, 6f).OnComplete(() =>
                        {
                            isJump = false;
                        });
                    });
                });
            });
        }
    }
}