using UnityEngine;

public class NewEnemy : EnemyBase
{
    [SerializeField] private int _countToDivide;
    [SerializeField] EnemyBase _enemy;

    protected override void Die()
    {
        base.Die();
        for (int i = 0; i < _countToDivide; i++)
        {
            var spawnedEnemy = Instantiate(_enemy, transform.position + SpawnOffset(), Quaternion.identity);
            
            _waveSpawner.SpawnedEnemies.Add(spawnedEnemy);
        }
    }

    private Vector3 SpawnOffset()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        Vector3 randomOffset = new Vector3(randomX, 0f, randomZ);
        return randomOffset;
    }
}