using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, ITarget
{
    [SerializeField] private Vector3 centerPosition; 
    public Vector3 Position => TargetTransform.position;

    public Transform TargetTransform => this.transform;

    public TargetStat TargetStart => throw new System.NotImplementedException();

    public Vector3 CenterPosition 
    {
        get 
        {
            centerPosition = Position;
            centerPosition.y = 1f;
            return centerPosition;
        } 
    }
}
