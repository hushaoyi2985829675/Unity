using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("摇摆范围")] [SerializeField] private float minAngle = 0.5f; // 最小旋转角度（度）
    [SerializeField] private float maxAngle = 1.2f; // 最大旋转角度（度）
    [SerializeField] private float moveRange = 0.3f; // 左右移动范围（较小的值更自然）

    [Header("动画参数")] [SerializeField] private float baseDuration = 1.5f;
    [SerializeField] private Ease swingEase = Ease.InOutSine; // 平滑缓动曲线

    private Tween moveTween; // 移动动画引用
    private Tween rotateTween; // 旋转动画引用

    void Start()
    {
        StartSwing();
    }

    void StartSwing()
    {
        float targetRightX = transform.localPosition.x + moveRange;
        float targetRightZ = Random.Range(minAngle, maxAngle); // 右倾角度

        float targetLeftX = transform.localPosition.x - moveRange;
        float targetLeftZ = Random.Range(-maxAngle, -minAngle); // 左倾角度

        moveTween = transform.DOLocalMoveX(targetRightX, baseDuration)
            .SetEase(swingEase)
            .OnComplete(() =>
            {
                moveTween = transform.DOLocalMoveX(targetLeftX, baseDuration)
                    .SetEase(swingEase);
            });

        rotateTween = transform.DOLocalRotate(new Vector3(0, 0, targetRightZ), baseDuration)
            .SetEase(swingEase)
            .OnComplete(() =>
            {
                rotateTween = transform.DOLocalRotate(new Vector3(0, 0, targetLeftZ), baseDuration)
                    .SetEase(swingEase).OnComplete(StartSwing);
            });
    }

    private void OnDestroy()
    {
        // 清理动画，防止内存泄漏
        moveTween?.Kill();
        rotateTween?.Kill();
    }
}