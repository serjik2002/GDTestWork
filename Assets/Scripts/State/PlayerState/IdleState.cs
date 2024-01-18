using UnityEngine;

public class IdleState : IState
{
    private Animator _animator;

    public IdleState(Animator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {
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
