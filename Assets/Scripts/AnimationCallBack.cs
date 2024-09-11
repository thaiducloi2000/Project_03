using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationEventCallbackData
{
    public UnityEvent callback;
}

public class AnimationCallBack : MonoBehaviour
{
    [SerializeField] private AnimationEventCallbackData[] animationCallbacks;
    int animationCallbacksCount = 0;

    private void Awake()
    {
        animationCallbacksCount = animationCallbacks.Length;
    }

    public void DoAnimationCallback(int indexCallback)
    {
        if (indexCallback > animationCallbacksCount - 1) return;
        animationCallbacks[indexCallback].callback?.Invoke();
    }

    public void DoAnimationCallback(AnimationEventCallbackData eventCallback)
    {
        eventCallback.callback?.Invoke();
    }

}
