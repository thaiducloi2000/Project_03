public class AI_DeathState : Enemy_ActionableState
{
    public AI_DeathState(Enemy enemy) : base(enemy)
    {
        _enemy = enemy;
        stateID = -1;
    }

    public override void Enter()
    {
        _enemy.AnimatorController.DoAnimation(stateID, true);
        _enemy.MoveToTarGet(true);
    }

    public override void Exit()
    {
        _enemy.AnimatorController.DoAnimation(stateID);
    }

    public override void Update()
    {
        return;
    }
}
