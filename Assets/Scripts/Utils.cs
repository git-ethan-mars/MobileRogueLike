using System;
using System.Collections;
using UnityEngine;

public static class Utils
{
    public static Rect GetScreenCoordinates(RectTransform uiElement)
    {
        var worldCorners = new Vector3[4];
        uiElement.GetWorldCorners(worldCorners);
        var result = new Rect(
            worldCorners[0].x,
            worldCorners[0].y,
            worldCorners[2].x - worldCorners[0].x,
            worldCorners[2].y - worldCorners[0].y);
        return result;
    }
    public static IEnumerator DoActionAfterDelay(float delayInSeconds, Action action)
    {
        yield return new WaitForSeconds(delayInSeconds);
        action();
    }

    public static IEnumerator DoActionAfterAnimationFinished(Animator animator, string animationName, Action action)
    {
        yield return new WaitForAnimationToFinish(animator, animationName);
        action();
    }

    private class WaitForAnimationToFinish : CustomYieldInstruction
    {
        private readonly string _animationName;

        private readonly Animator _animator;
        private readonly int _layerIndex;


        private AnimatorStateInfo StateInfo => _animator.GetCurrentAnimatorStateInfo(_layerIndex);

        private bool CorrectAnimationIsPlaying => StateInfo.IsName(_animationName);

        private bool AnimationIsDone => StateInfo.normalizedTime >= 1;

        public override bool keepWaiting => CorrectAnimationIsPlaying && !AnimationIsDone;

        public WaitForAnimationToFinish(Animator animator, string animationName, int layerIndex = 0)
        {
            _animator = animator;
            _layerIndex = layerIndex;
            _animationName = animationName;
        }
    }
}