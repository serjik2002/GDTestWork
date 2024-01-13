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
    [SerializeField] private float _atackCooldown;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _rotationSpeed = 10;

    [SerializeField] private Button _attackButton;

    private float lastAttackTime = 0;
    private bool isDead = false;
    private bool _isWalking = false;

    private StateMachine _stateMachine = new StateMachine();

    public Animator AnimatorController;

    public UnityEvent OnPlayerDie;

    private void Start()
    {
        OnPlayerDie.AddListener(Die);
        _stateMachine.Initialize(new IdleState(AnimatorController));
        _attackButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState(new AttackState(AnimatorController));
        });
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (_healthPoint <= 0)
        {
            OnPlayerDie.Invoke();
        }

        //Move();

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // ƒобавлено условие дл€ перехода в состо€ние WalkState
        if (horizontal != 0 || vertical != 0)
        {
            if (!_isWalking)
            {
                _stateMachine.ChangeState(new WalkState(AnimatorController));
                _isWalking = true;
            }
        }
        else
        {
            if (_isWalking)
            {
                _stateMachine.ChangeState(new IdleState(AnimatorController));
                _isWalking = false;
            }
        }

        _stateMachine.CurrentState.Update();

    }

    public void Attack()
    {
        var enemies = GameManager.Instance.Enemies;
        enemies.RemoveAll(item => item == null);
        Enemie closestEnemie = null;
        float distance = 0, closestDistance = 0;

        if (enemies.Count == 0)
            return;

        closestEnemie = enemies[0];

        for (int i = 1; i < enemies.Count; i++)
        {

            distance = Vector3.Distance(transform.position, enemies[i].transform.position);
            closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemies[i];
            }

        }

        if (closestDistance <= _attackRange)
        {
            if (Time.time - lastAttackTime > _atackCooldown)
            {
                transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                lastAttackTime = Time.time;
                closestEnemie.TakeDamage(_damage);
                AnimatorController.SetTrigger("Attack");
            }
        }

    }

    public void TakeDamage(float damage)
    {
        _healthPoint -= damage;
    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        GameManager.Instance.GameOver();
    }

    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        Vector3 lastDirection = direction;

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

}
