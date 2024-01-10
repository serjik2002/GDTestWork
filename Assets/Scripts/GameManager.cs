using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Player _player;
    [SerializeField] private List<Enemie> _enemies = new List<Enemie>();
    [SerializeField] private GameObject _lose;
    [SerializeField] private GameObject _win;
    
    [SerializeField] private LevelConfig Config;
    
    private int currWave = 0;


    public Player Player => _player;
    public List<Enemie> Enemies => _enemies;
    public GameObject Lose => _lose;
    public GameObject Win => _win;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        SpawnWave();
    }

    public void AddEnemie(Enemie enemie)
    {
        _enemies.Add(enemie);
    }

    public void RemoveEnemie(Enemie enemie)
    {
        _enemies.Remove(enemie);
        if(_enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    public void GameOver()
    {
        _lose.SetActive(true);
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            _win.SetActive(true);
            return;
        }

        var wave = Config.Waves[currWave];
        foreach (var character in wave.Characters)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(character, pos, Quaternion.identity);
        }
        currWave++;

    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        _enemies.Clear();
    }
    

}
