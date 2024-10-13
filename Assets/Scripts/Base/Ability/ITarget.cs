using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TargetStat
{
    float heath;
    float ammor;
    float attackSpeed;
    float speed;
}

public interface ITarget
{
    public Vector3 CenterPosition { get; }
    public Vector3 Position { get;}
    public Transform TargetTransform { get;}
    public TargetStat TargetStart { get; }
}
