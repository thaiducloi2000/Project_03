using System;
public abstract class Enemy_ActionableState : IState
{
    protected Enemy _enemy;
    protected int stateID;

    public Enemy_ActionableState(Enemy enemy, int idState = -1)
    {
        _enemy = enemy;
        stateID = idState;
    }

    /// <summary>
    /// Action When Enter State
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// Action When Exit State
    /// </summary>
    public abstract void Exit();

    /// <summary>
    /// Check Condition To Exit Current State
    /// </summary>
    public abstract void Update();
}
