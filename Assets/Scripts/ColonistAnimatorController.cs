using UnityEngine;
using UnityEngine.InputSystem;

public class ColonistAnimatorController : MonoBehaviour
{
    /// <summary>
    /// 住人のアニメーター
    /// </summary>
    public Animator CollonistAnimator;

    private void Update()
    {
        // デバッグ用
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            PlayIdleAnimation();
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            PlayWalkingAnimation();
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            PlayMineAnimation();
        }
         if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            PlaySleepingAnimation();
        }
    }
    /// <summary>
    /// 待機アニメーションを再生
    /// </summary>
    public void PlayIdleAnimation()
    {
        CollonistAnimator.SetInteger("AnimationState", 0);
    }
    /// <summary>
    /// 歩くアニメーションを再生
    /// </summary>
    public void PlayWalkingAnimation()
    {
        CollonistAnimator.SetInteger("AnimationState", 1);
    }
    /// <summary>
    /// 掘るアニメーションを再生
    /// </summary>
    public void PlayMineAnimation()
    {
        CollonistAnimator.SetInteger("AnimationState", 2);
    }
    /// <summary>
    /// 掘るアニメーションを再生
    /// </summary>
    public void PlaySleepingAnimation()
    {
        CollonistAnimator.SetInteger("AnimationState", 3);
    }
}
