using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private LevelConfig _config;

    private int _currentWave;
    private List<EnemyBase> _enemies;
    private List<EnemyBase> _spawnedEnemies = new List<EnemyBase>();

    public List<EnemyBase> Enemies => _enemies;
    public List<EnemyBase> SpawnedEnemies => _spawnedEnemies;
    public int CurrentWave => _currentWave;

    public UnityEvent OnWaveCompleted;
    public UnityEvent OnPlayerWin;

    private void Start()
    {
        _currentWave = 0;
        OnWaveCompleted.AddListener(() => 
        {
            if (_currentWave < _config.Waves.Length - 1)
            {
                _currentWave++;
                SpawnWave();
            }
            else
            {
                OnPlayerWin.Invoke();
            }

        });
        SpawnWave();    
    }

    private void Update()
    {
        if(SpawnedEnemies.Count == 0)
            OnWaveCompleted.Invoke();
        if (_currentWave == _config.Waves.Length)
        {
            OnPlayerWin.Invoke();
        }
    }

    private void SpawnWave()
    {
        GetNewWave();
        if(_currentWave != _config.Waves.Length)
        {
            var wave = _config.Waves[_currentWave];
            foreach (var enemie in wave.Enemies)
            {
                Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                var spawnedEnemy = Instantiate(enemie, randomPosition, Quaternion.identity);
                _spawnedEnemies.Add(spawnedEnemy);
            }
        }
        
    }

    private void GetNewWave()
    {
        _enemies = _config.Waves[_currentWave].Enemies;
    }    

    public void RemoveEnemyFromWave(EnemyBase enemie)
    {
        _spawnedEnemies.Remove(enemie);
    }
}
