using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayerState : IState
{
    private EnemyBase _enemie;

    public MoveToPlayerState(EnemyBase enemie)
    {
        this._enemie = enemie;
    }
    public void Enter()
    {
        _enemie.Agent.isStopped = false;
        _enemie.Animator.SetFloat("Speed", _enemie.Agent.speed);
    }

    public void Exit()
    {
        _enemie.Agent.isStopped = true;
        _enemie.Animator.SetFloat("Speed", 0);
        return;
    }

    public void Update()
    {
        _enemie.Agent.SetDestination(GameManager.Instance.Player.transform.position);
    }
}