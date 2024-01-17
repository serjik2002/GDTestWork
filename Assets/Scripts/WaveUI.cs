using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private LevelConfig _config;
    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private TMP_Text _waveText;

    private const string _waveTextKey = "Current wave";

    private void Start()
    {
        UpdateText();
        _waveSpawner.OnWaveCompleted.AddListener(UpdateText);
    }

    private void UpdateText()
    {
        _waveText.text = _waveTextKey + " " + _waveSpawner.CurrentWave + 1 + "/" + _config.Waves.Length;
    }
}
