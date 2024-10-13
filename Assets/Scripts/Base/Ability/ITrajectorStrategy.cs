using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrajectorStrategy
{
    public Vector3[] Directions(Vector3 direction);
}

public abstract class TrajectorShoot : ITrajectorStrategy
{
    protected int _amount;

    public TrajectorShoot(ref int amount)
    {
        _amount = amount;
    }

    public abstract Vector3[] Directions(Vector3 direction);
}

public class SingleTrajector : TrajectorShoot
{
    public SingleTrajector(int amount = 1) : base(ref amount)
    {
    }

    public override Vector3[] Directions(Vector3 direction)
    {
        return new Vector3[] { direction };
    }
}

public class SpreadTrajector : TrajectorShoot
{
    protected float _maxAngles;
    protected Vector3[] directions;
    private float max;
    private float min;

    private float angleStep;

    public SpreadTrajector(int amount, float maxAngles) : base(ref amount)
    {
        _maxAngles = maxAngles;
    }

    public override Vector3[] Directions(Vector3 direction)
    {
        if(directions == null || directions.Length == 0)
        {
            directions = new Vector3[_amount];
        }

        max = _maxAngles / 2;
        min = -max;

        angleStep = _maxAngles <= 180 ? (_maxAngles / (_amount - 1)) : (360f / _amount); ;

        for (int i = 0; i < _amount; i++)
        {
            float angle = Mathf.Clamp(min + i * angleStep, min, max);
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * direction;
            directions[i] = dir;
        }

        return directions;
    }
}
