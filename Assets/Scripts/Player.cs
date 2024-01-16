using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _healthPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _superAttackCooldown = 2f;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _superAttackButton;


    private float lastAttackTime = 0;
    private bool isDead = false;
    private float horizontal, vertical;
    private StateMachine _stateMachine = new StateMachine();


    public UnityEvent OnPlayerDie;

    public Animator AnimatorController;

    public bool IsWalking { get; set; }
    public float Speed => _speed;
    public StateMachine StateMachine => _stateMachine;

    private void Start()
    {
        IsWalking = false;
        OnPlayerDie.AddListener(Die);
        _stateMachine.Initialize(new IdleState());
        _attackButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState(new AttackState(AnimatorController));
        });
        _superAttackButton.onClick.AddListener(() =>
        {
            StartCoroutine(SuperAttackButtonHandle());
        });
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        HandleDeath();
        HandleMovement();

        _stateMachine.CurrentState.Update();

    }

    private void HandleMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            if (!IsWalking)
            {
                _stateMachine.ChangeState(new WalkState(AnimatorController));
                IsWalking = true;
            }
        }
        else if (IsWalking)
        { 
        
            _stateMachine.ChangeState(new IdleState());
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

    public void Attack()
    {
        Enemie closestEnemy = FindClosestEnemy();
        Debug.Log(closestEnemy);
        if (closestEnemy != null)
        {
            bool canAttack = CanAttack(closestEnemy);
            Debug.Log(canAttack);
            if (canAttack)
            {
                PerformAttack(closestEnemy);
            }
        }
        else
        {
            AnimatorController.SetTrigger("Attack");
        }
    }

    public void SuperAttack()
    {

    }

    private void PerformAttack(Enemie enemy)
    {
        transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);

        lastAttackTime = Time.time;
        enemy.TakeDamage(_damage);
        AnimatorController.SetTrigger("Attack");
    }

    private Enemie FindClosestEnemy()
    {
        var enemies = GameManager.Instance.WaveSpawner.SpawnedEnemies;
        Debug.Log(enemies.Count);
        Enemie closestEnemie = enemies[0];
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
        Debug.Log(closestEnemie.transform.position);
        return closestEnemie;
    }

    private bool CanAttack(Enemie enemy)
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

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");
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

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, _attackRange);

    }

    private IEnumerator SuperAttackButtonHandle()
    {
        _superAttackButton.interactable = false;
        yield return new WaitForSeconds(_superAttackCooldown);
        _superAttackButton.interactable = true;
    }
}
