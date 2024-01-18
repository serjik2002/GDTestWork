using UnityEngine;

public class DieState : IState
{
    private Player _player;

    public DieState()
    {
        _player = GameManager.Instance.Player;
    }

    public void Enter()
    {
        Debug.Log("Die state");
        _player.Die();
    }

    public void Exit()
    {
        return;
    }

    public void Update()
    {
        return;
    }
}