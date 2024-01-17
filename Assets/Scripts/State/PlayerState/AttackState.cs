using UnityEngine;

public class AttackState : IState
{
    private Player _player;

    public AttackState()
    {
        _player = GameManager.Instance.Player;
    }

    public void Enter()
    {
       // Debug.Log("Enter Attack state");
        _player.Animator.SetTrigger("Attack");
        GameManager.Instance.Player.IsWalking = false;
        GameManager.Instance.Player.Attack(GameManager.Instance.Player.Damage);
    }

    public void Exit()
    {
        _player.Animator.ResetTrigger("Attack");
    }

    public void Update()
    {
        return;
    }
}
