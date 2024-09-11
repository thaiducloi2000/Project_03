using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemySetting
{
    /// <summary>
    /// Prefab to Spawn
    /// </summary>
    public Enemy prefab;

    /// <summary>
    /// Start Time that enemy will Appear (0p -> 1p -> 3p -> 5p -> 10p -> 15p) 
    /// </summary>
    public float timeAppear;

    public int defaultCapacity;

    public int maxCapacity;

    public int maxAppaerInSameTime;

    public float additionRate;

    public void IncreaseRate(float rate)
    {
        additionRate += rate;
    }
}
[CreateAssetMenu(fileName = "WaveSetting", menuName = "GameSetting/Wave", order = 1)]
public class WaveSetting : ScriptableObject
{
    //[SerializeField] private int maxEnemyOnMap = 100;
    [SerializeField] private EnemySetting[] enemys;

    public EnemySetting[] GetEnemys => enemys;


    /// <summary>
    /// Callback when an action rate change
    /// </summary>
    /// <param name="rate"></param>
    public void IncreaseRate(float rate)
    {
        foreach(EnemySetting enemy in enemys)
        {
            enemy.IncreaseRate(rate);
        }
    }
}
