using UnityEngine;

public class IdleState : IState
{
    private Player _player;

    public IdleState()
    {
        _player = GameManager.Instance.Player;
    }

    public void Enter()
    {
        //Debug.Log("Enter Idle State");
        _player.Animator.SetTrigger("Idle");
    }

    public void Exit()
    {
        _player.Animator.ResetTrigger("Idle");
    }

    public void Update()
    {
        return;
    }
}
