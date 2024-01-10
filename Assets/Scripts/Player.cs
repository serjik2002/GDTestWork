using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private float _healthPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _atackSpeed;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _rotationSpeed = 10;

    private float lastAttackTime = 0;
    private bool isDead = false;

    private StateMachine _stateMachine = new StateMachine();

    public Animator AnimatorController;

    public UnityEvent OnPlayerDie;

    private void Start()
    {
        OnPlayerDie.AddListener(Die);
        _stateMachine.Initialize(new IdleState(AnimatorController));
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
            _stateMachine.ChangeState(new WalkState(AnimatorController));
        }
        else
        {
            _stateMachine.ChangeState(new IdleState(AnimatorController));
        }
        _stateMachine.CurrentState.Update();

        var enemies = GameManager.Instance.Enemies;
        Enemie closestEnemie = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                continue;
            }

            if (closestEnemie == null)
            {
                closestEnemie = enemies[i];
                continue;
            }

            var distance = Vector3.Distance(transform.position, enemies[i].transform.position);
            var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemies[i];
            }

        }

        if (closestEnemie != null)
        {
            var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
            if (distance <= _attackRange)
            {
                if (Time.time - lastAttackTime > _atackSpeed)
                {
                    //transform.LookAt(closestEnemie.transform);
                    transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                    lastAttackTime = Time.time;
                    closestEnemie.TakeDamage(_damage);
                    AnimatorController.SetTrigger("Attack");
                }
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

}
