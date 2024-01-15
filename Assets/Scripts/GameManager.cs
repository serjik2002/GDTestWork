using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Player _player;
    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private List<Enemie> _enemies = new List<Enemie>();
    [SerializeField] private GameObject _lose;
    [SerializeField] private GameObject _win;
    
    [SerializeField] private LevelConfig Config;
    
    private int currWave = 0;


    public Player Player => _player;
    public List<Enemie> Enemies => _enemies;
    public WaveSpawner WaveSpawner => _waveSpawner;
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

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        _enemies.Clear();
    }
    

}
