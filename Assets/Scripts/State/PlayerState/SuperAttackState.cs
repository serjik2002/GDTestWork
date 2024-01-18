using UnityEngine;

public class SuperAttackState : IState
{
    private Player _player;

    public SuperAttackState()
    {
        _player = GameManager.Instance.Player;
    }
    public void Enter()
    {
        _player.Animator.SetTrigger("SuperAttack");
        GameManager.Instance.Player.IsWalking = false;
        GameManager.Instance.Player.SuperAttack();
    }

    public void Exit()
    {
        _player.Animator.ResetTrigger("SuperAttack");
    }

    public void Update()
    {
        return;
    }
}