using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TimeSpan timePlaying;
    private bool isPause;
    private float elapsedTime;
    private float startTime;

    public void BeginTimer() 
    {
        isPause = false;
        elapsedTime = 0f;
        startTime = Time.time;

        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (!isPause)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            yield return null;
        }
    }

    private void OnDisable()
    {
        isPause = true;
        StopAllCoroutines();
    }
}
