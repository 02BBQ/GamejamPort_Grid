using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeController : MonoSingleton<TimeController>
{
    public void SetTimeFreeze(float freezeValue, float beforeDelay, float freezeTime)
    {
        StopAllCoroutines();

        StartCoroutine(TimeFreezeCoroutine(freezeValue, beforeDelay, () =>
        {
            StartCoroutine(TimeFreezeCoroutine(1f, freezeTime));
        }));
    }

    private IEnumerator TimeFreezeCoroutine(float freezeValue, float beforeDelay, Action Callback = null)
    {
        yield return new WaitForSecondsRealtime(beforeDelay);
        yield return new WaitUntil(() => !UIHandler.Instance.LevelingFinished);
        Time.timeScale = freezeValue;
        Callback?.Invoke();
    }
}
