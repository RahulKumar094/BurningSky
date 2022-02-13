using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get { return instance; } }
    private static EnemySpawnManager instance;

    public Level[] LevelData = new Level[Game.LevelMax];
    
    private const float sequenceInterval = 0.25f;

    [SerializeField]
    private int currentSequenceIndex = -1;
    private LevelData currentLevelData;
    private float interval;

    private IEnumerator routine;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void StartSpawner(bool startOver = false)
    {
        if (startOver)
        {
            currentSequenceIndex = 0;
            currentLevelData = LevelData[Game.Level - 1].LevelData;
            interval = currentLevelData.AverageLevelTimeInSec / currentLevelData.SpawnSequences.Count;
        }

        routine = UpdateManager();
        StartCoroutine(routine);
    }

    public void StopSpawner()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    private IEnumerator UpdateManager()
    {
        //init varaibles for small green planes
        float randomMultiplier = 0f;
        float rotationDelay = 0f;

        while (currentSequenceIndex < currentLevelData.SpawnSequences.Count)
        {
            SpawnSequence sequence = currentLevelData.SpawnSequences[currentSequenceIndex];

            float randomX = Random.Range(Game.XAxisSpawnRange.min, Game.XAxisSpawnRange.max);

            if (sequence.type == EnemyType.SmallGreen)
            {
                randomMultiplier = Random.Range(-1, 2);
                rotationDelay = Random.Range(0.5f, 1f);
            }

            for (int i = 0; i < sequence.count; i++)
            {
                EnemyPlane plane = ObjectPool.Instance.GetEnemyPlane(sequence.type);
                plane.SpawnAt(new Vector3(randomX, Game.EnemySpawnPosition.y, Game.EnemySpawnPosition.z));

                if (plane.type == EnemyType.SmallGreen)
                {
                    SmallGreenEnemyPlane smallGreenEnemyPlane = plane as SmallGreenEnemyPlane;
                    smallGreenEnemyPlane.SetRotation(randomMultiplier, rotationDelay);
                }
                yield return new WaitForSeconds(sequenceInterval);
            }
            currentSequenceIndex++;
            yield return new WaitForSeconds(interval);
        }
    }

    public int MaxCoinInLevel(int levelID)
    {
        int maxValueInLevel = 0;

        List<EnemyPlane> samples = ObjectPool.EnemySamples;
        List<SpawnSequence> SpawnSequences = LevelData[levelID].LevelData.SpawnSequences;
        foreach (SpawnSequence seq in SpawnSequences)
        {
            EnemyPlane plane = samples.Find(x => x.type == seq.type);
            maxValueInLevel += plane.GetTotalCoinValue() * seq.count;
        }
        return maxValueInLevel;
    }
}