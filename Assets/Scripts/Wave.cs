using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Data/Waves")]
[Serializable]
public class Wave : ScriptableObject
{
    [SerializeField] private List<EnemyBase> _enemies;

    public List<EnemyBase> Enemies => _enemies;


}
