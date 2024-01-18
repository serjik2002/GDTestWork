using UnityEngine;

public class WalkState : IState
{
    private Player _player;

    public WalkState()
    {
        _player = GameManager.Instance.Player;
    }

    public void Enter()
    {
        _player.Animator.SetFloat("Speed", GameManager.Instance.Player.Speed);
    }

    public void Exit()
    {
        _player.Animator.SetFloat("Speed", 0);
    }

    public void Update()
    {
        GameManager.Instance.Player.Move();
    }
}
