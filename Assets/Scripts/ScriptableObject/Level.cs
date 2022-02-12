using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Level Data")]
public class Level : ScriptableObject
{
    public LevelData LevelData = new LevelData();
}

[Serializable]
public class LevelData
{
    [SerializeField]
    public float AverageLevelTimeInSec;
    [SerializeField]
    public List<SpawnSequence> SpawnSequences = new List<SpawnSequence>();
}

[Serializable]
public struct SpawnSequence
{
    [SerializeField]
    public EnemyType type;
    [SerializeField]
    public int count;
}