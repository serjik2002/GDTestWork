using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Player _player;
    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;
     

    public Player Player => _player;
    public WaveSpawner WaveSpawner => _waveSpawner;
    public GameObject Lose => _losePanel;
    public GameObject Win => _winPanel;

    private void Start()
    {
        _player.OnPlayerDie.AddListener(OpenLosePanel);
        _waveSpawner.OnPlayerWin.AddListener(OpenWinPanel);
    }

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

    private void OpenLosePanel()
    {
        _losePanel.SetActive(true);
    }

    private void OpenWinPanel()
    {
        _winPanel.SetActive(true);
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    

}
