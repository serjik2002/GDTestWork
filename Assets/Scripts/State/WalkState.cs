using UnityEngine;

public class WalkState : IState
{
    private Animator _animator;

    public WalkState(Animator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {
        //Debug.Log("Enter Walk State");
        //_animator.SetTrigger("Walk");
        _animator.SetFloat("Speed", GameManager.Instance.Player.Speed);
    }

    public void Exit()
    {
        _animator.SetFloat("Speed", 0);
    }

    public void Update()
    {
        GameManager.Instance.Player.Move();
    }
}
