﻿using UnityEngine;

public class AttackPlayerState : IState
{
    private Enemie _enemie;

    public AttackPlayerState(Enemie enemie)
    {
        this._enemie = enemie;
    }
    public void Enter()
    {
        Debug.Log("Enter Attack player state");
        _enemie.Animator.SetTrigger("Attack");
    }

    public void Exit()
    {
        return;
    }

    public void Update()
    {
        _enemie.Attack();
    }
}
