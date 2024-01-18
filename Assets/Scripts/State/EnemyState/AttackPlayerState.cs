using UnityEngine;

public class AttackPlayerState : IState
{
    private EnemyBase _enemie;

    public AttackPlayerState(EnemyBase enemie)
    {
        this._enemie = enemie;
    }
    public void Enter()
    {
        return;
    }

    public void Exit()
    {
        return;
    }

    public void Update()
    {
        _enemie.Attack();
    }
}
