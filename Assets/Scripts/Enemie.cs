using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    [SerializeField] private float _healthPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _atackSpeed;
    [SerializeField] private float _attackRange = 2;


    public Animator AnimatorController;
    public NavMeshAgent Agent;
    


    private float lastAttackTime = 0;
    private bool isDead = false;
    public float HealthPoint => _healthPoint;

    private void Start()
    {
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

        var distance = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
     
        if (distance <= _attackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > _atackSpeed)
            {
                lastAttackTime = Time.time;
                GameManager.Instance.Player.TakeDamage(_damage);
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.SetDestination(GameManager.Instance.Player.transform.position);
        }
        AnimatorController.SetFloat("Speed", Agent.speed); 
        Debug.Log(Agent.speed);

    }

    public void TakeDamage(float damage)
    {
        _healthPoint -=damage;
    }



    private void Die()
    {
        GameManager.Instance.RemoveEnemie(this);
        isDead = true;
        AnimatorController.SetTrigger("Die");
    }

}
