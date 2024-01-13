using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    [SerializeField] private float _healthPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _atackCooldown;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private Animator _animator;

    public NavMeshAgent Agent;
    
    private StateMachine _stateMachine = new StateMachine();

    private float _lastAttackTime = 0;
    private bool isDead = false;
    private bool isAttacked = false;

    public Animator Animator => _animator;
    public float HealthPoint => _healthPoint;
    public float Damage => _damage;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _atackCooldown;
    public float LastAttackTime => _lastAttackTime;

    private void Start()
    {
        _stateMachine.Initialize(new MoveToPlayerState(Agent));
        GameManager.Instance.AddEnemie(this);
        Agent.SetDestination(GameManager.Instance.Player.transform.position);
    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        if (_healthPoint <= 0)
        {
            Die();
            Agent.isStopped = true;
            return;
        }

        //var distance = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);

        _stateMachine.CurrentState.Update();
        if (!Agent.pathPending && Agent.remainingDistance < _attackRange)
        {
            if (!isAttacked)
            {
                _stateMachine.ChangeState(new AttackPlayerState(this));
                isAttacked = true;
            }
        }
        else
        {
            _stateMachine.ChangeState(new MoveToPlayerState(Agent));
            isAttacked = false;
        }
        Animator.SetFloat("Speed", Agent.speed);

    }

    public void TakeDamage(float damage)
    {
        _healthPoint -=damage;
    }



    private void Die()
    {
        GameManager.Instance.RemoveEnemie(this);
        isDead = true;
        Animator.SetTrigger("Die");
    }

    public void Attack()
    {
        if (Time.time - _lastAttackTime > _atackCooldown)
        {
            _lastAttackTime = Time.time;
            GameManager.Instance.Player.TakeDamage(_damage);
        }
    }

}
