using Unity.IO.LowLevel.Unsafe;
using UnityEngine.Rendering;

public class AI_RespawnState : Enemy_ActionableState
{
    public AI_RespawnState(Enemy enemy) : base(enemy)
    {
        _enemy = enemy;
        stateID = 0;
    }

    public override void Enter()
    {
        _enemy.AnimatorController.DoAnimation(stateID, true);
    }

    public override void Exit()
    {
        _enemy.AnimatorController.DoAnimation(stateID);
    }

    public override void Update()
    {
        if(_enemy.HP <= 0)
        {
            _enemy.state.TransitionTo(typeof(AI_DeathState));
        }
    }
}
