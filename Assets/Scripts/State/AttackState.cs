using UnityEngine;

public class AttackState : IState
{
    private Animator _animator;

    public AttackState(Animator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {
       // Debug.Log("Enter Attack state");
        _animator.SetTrigger("Attack");
        GameManager.Instance.Player.IsWalking = false;
        GameManager.Instance.Player.Attack();
    }

    public void Exit()
    {
        _animator.ResetTrigger("Attack");
    }

    public void Update()
    {
        return;
    }
}