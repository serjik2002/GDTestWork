using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayerState : IState
{
    private Enemie _enemie;

    public MoveToPlayerState(Enemie enemie)
    {
        this._enemie = enemie;
    }
    public void Enter()
    {
        Debug.Log("Enter Move to player state");
        _enemie.Agent.isStopped = false;
        _enemie.Animator.SetTrigger("Walk");
    }

    public void Exit()
    {
        _enemie.Agent.isStopped = true;
        _enemie.Animator.ResetTrigger("Walk");
        return;
    }

    public void Update()
    {
        _enemie.Agent.SetDestination(GameManager.Instance.Player.transform.position);
    }
}