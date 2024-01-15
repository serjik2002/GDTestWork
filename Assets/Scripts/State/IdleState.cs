using UnityEngine;

public class IdleState : IState
{
    private Animator _animator;

    public IdleState()
    {
        _animator = GameManager.Instance.Player.AnimatorController;
    }

    public void Enter()
    {
        //Debug.Log("Enter Idle State");
        _animator.SetTrigger("Idle");
    }

    public void Exit()
    {
        _animator.ResetTrigger("Idle");
    }

    public void Update()
    {
        return;
    }
}
