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
        Debug.Log("Enter Walk State");
        _animator.SetTrigger("Walk");
    }

    public void Exit()
    {
        Debug.Log("Exit walk state");
    }

    public void Update()
    {
        GameManager.Instance.Player.Move();
    }
}
