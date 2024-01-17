using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private LevelConfig _config;

    private int _currentWave;
    private List<Enemie> _enemies;
    private List<Enemie> _spawnedEnemies = new List<Enemie>();

    public List<Enemie> Enemies => _enemies;
    public List<Enemie> SpawnedEnemies => _spawnedEnemies;
    public int CurrentWave => _currentWave;

    public UnityEvent OnWaveCompleted;

    private void Start()
    {
        _currentWave = 0;
        OnWaveCompleted.AddListener(SpawnWave);
        SpawnWave();    
    }

    private void Update()
    {
        if(SpawnedEnemies.Count == 0)
            OnWaveCompleted.Invoke();
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
        _currentWave++;
    }

    private void GetNewWave()
    {
        _enemies = _config.Waves[_currentWave].Enemies;
    }    

    public void RemoveEnemyFromWave(Enemie enemie)
    {
        _spawnedEnemies.Remove(enemie);
    }
}
