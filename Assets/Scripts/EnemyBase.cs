using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private float _healthPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _atackCooldown;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _rewardHpToDie = 2;

    private StateMachine _stateMachine = new StateMachine();

    protected WaveSpawner _waveSpawner;
    private float _lastAttackTime = 0;
    private bool _isAttacked = false;
    private bool _isWalking = true;

    public NavMeshAgent Agent => _agent;
    public Animator Animator => _animator;
    public float HealthPoint => _healthPoint;
    public float Damage => _damage;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _atackCooldown;
    public float LastAttackTime => _lastAttackTime;

    private void Start()
    {
        _waveSpawner = GameManager.Instance.WaveSpawner;
        _agent = GetComponent<NavMeshAgent>();
        _stateMachine.Initialize(new MoveToPlayerState(this));
        GameManager.Instance.Player.OnPlayerDie.AddListener(() =>
        {
            _stateMachine.ChangeState(new IdleState(_animator));
        });
    }

    private void Update()
    {
        if (_healthPoint <= 0)
        {
            Die();
            return;
        }

        if (GetDistanceToPlayer() < _attackRange)
        {
            if (!_isAttacked)
            {
                _stateMachine.ChangeState(new AttackPlayerState(this));
                _isAttacked = true;
                _isWalking = false;
            }
        }
        else if(!_isWalking)
        {
            _stateMachine.ChangeState(new MoveToPlayerState(this));
            _isWalking = true;
            _isAttacked = false;
        }
        _stateMachine.CurrentState.Update();

    }

    public void TakeDamage(float damage)
    {
        _healthPoint -= damage;
    }

    protected virtual void Die()
    {
        GameManager.Instance.Player.AddHealth(_rewardHpToDie);
        _waveSpawner.RemoveEnemyFromWave(this);
        Animator.SetTrigger("Die");
        Destroy(gameObject);
    }

    public virtual void Attack()
    {
        float distance = GetDistanceToPlayer();
        if (distance <= _attackRange)
        {
            if (Time.time - _lastAttackTime > _atackCooldown)
            {
                _animator.SetTrigger("Attack");
                _lastAttackTime = Time.time;
                GameManager.Instance.Player.TakeDamage(_damage);
            }
        }

    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
    }
}
