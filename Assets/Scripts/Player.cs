using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _healthPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _superAttackCooldown = 2f;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _rotationSpeed = 10;
    [Space(10)]
    [Header("Other")]
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _superAttackButton;
    [SerializeField] private Animator _animator;


    private float lastAttackTime = 0;
    private bool isDead = false;
    private bool _isSuperAttackEnabled = true;
    private float horizontal, vertical;
    private StateMachine _stateMachine = new StateMachine();



    public bool IsWalking { get; set; }
    public float Speed => _speed;
    public float Damage => _damage;
    public StateMachine StateMachine => _stateMachine;
    public Animator Animator => _animator;

    public UnityEvent OnPlayerDie;

    private void Start()
    {
        IsWalking = false;
        
        _stateMachine.Initialize(new IdleState(_animator));
        
        OnPlayerDie.AddListener(() =>
        {
            _stateMachine.ChangeState(new DieState());
        });

        _attackButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState(new AttackState());
        });
        
        _superAttackButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState(new SuperAttackState());
            StartCoroutine(SuperAttackButtonHandle());
        });
    }

    private void Update()
    {
        if (!isDead)
        {
            HandleDeath();
            HandleMovement();

            _stateMachine.CurrentState.Update();
            bool enemiesNearby = CheckForEnemiesInRadius();
            _superAttackButton.interactable = enemiesNearby && _isSuperAttackEnabled;
        }
    }

    private void HandleMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        if (horizontal != 0 || vertical != 0)
        {
            if (!IsWalking)
            {
                _stateMachine.ChangeState(new WalkState());
                IsWalking = true;
            }
        }
        else if (IsWalking)
        {
            _stateMachine.ChangeState(new IdleState(_animator)); ;
            IsWalking = false;
        }
    }

    private void HandleDeath()
    {
        if (_healthPoint <= 0)
        {
            OnPlayerDie.Invoke();
        }
    }

    public void Attack(float damage)
    {
        if (!isDead)
        {
            EnemyBase closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                bool canAttack = CanAttack(closestEnemy);
                if (canAttack)
                {
                    PerformAttack(closestEnemy, damage);
                }
            }
        }
        
    }

    public void SuperAttack()
    {
        Attack(_damage * 2);
    }

    private void PerformAttack(EnemyBase enemy , float damage)
    {
        RotateToEnemy(enemy);

        lastAttackTime = Time.time;
        enemy.TakeDamage(damage);
    }

    private void RotateToEnemy(EnemyBase enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 eulerAngles = rotation.eulerAngles;
        transform.eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
    }

    private EnemyBase FindClosestEnemy()
    {
        var enemies = GameManager.Instance.WaveSpawner.SpawnedEnemies;
        if (enemies.Count == 0)
        {
            return null;
        }
        EnemyBase closestEnemie = enemies[0];
        float distance = 0, closestDistance = 0;

        for (int i = 1; i < enemies.Count; i++)
        {

            distance = Vector3.Distance(transform.position, enemies[i].transform.position);
            closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemies[i];
            }

        }
        return closestEnemie;
    }

    private bool CanAttack(EnemyBase enemy)
    {
        if (Time.time - lastAttackTime > _attackCooldown)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            return distance <= _attackRange;
        }

        return false;
    }

    public void TakeDamage(float damage)
    {
        _healthPoint -= damage;
    }

    public void Die()
    {
        _animator.SetTrigger("Die");
        isDead = true;
        _attackButton.interactable = false;
        _isSuperAttackEnabled = false;
    }

    public void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);

        transform.position += direction.normalized * _speed * Time.deltaTime;


        if (horizontal != 0 || vertical != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);

        }
    }

    public void AddHealth(float healthPoint)
    {
        _healthPoint += healthPoint;
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, _attackRange);

    }

    private IEnumerator SuperAttackButtonHandle()
    {
        _superAttackButton.interactable = _isSuperAttackEnabled = false;
        yield return new WaitForSeconds(_superAttackCooldown);
        _superAttackButton.interactable = _isSuperAttackEnabled = true;
        
    }

    private bool CheckForEnemiesInRadius()
    {
        var enemies = GameManager.Instance.WaveSpawner.SpawnedEnemies;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= _attackRange)
            {
                return true; 
            }
        }
        return false; 
    }
}
