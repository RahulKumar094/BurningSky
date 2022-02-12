using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get { return instance; } }
    private static EnemySpawnManager instance;

    public Level[] LevelData = new Level[5];
    public MinMax XSpawnPositionRange = new MinMax(-8f, 8f);

    private static Vector3 spawnPosition = new Vector3(0, -20f, 22f);
    private const float sequenceInterval = 0.25f;
    private bool isPaused;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField]
    private int currentSequenceIndex;
    private LevelData currentLevelData;
    private float interval;
    public void StartSpawner(bool startOver = true)
    {
        isPaused = false;

        if (startOver)
        {
            currentSequenceIndex = 0;
            currentLevelData = LevelData[GameManager.Instance.Level - 1].LevelData;
            interval = currentLevelData.AverageLevelTimeInSec / currentLevelData.SpawnSequences.Count;
        }

        StartCoroutine("UpdateManager");
    }

    public void StopSpawner()
    {
        isPaused = true;
    }

    private IEnumerator UpdateManager()
    {
        //init varaibles for small green planes
        float randomMultiplier = 0f;
        float rotationDelay = 0f;

        while (!isPaused && currentSequenceIndex < currentLevelData.SpawnSequences.Count)
        {
            SpawnSequence sequence = currentLevelData.SpawnSequences[currentSequenceIndex];

            float randomX = Random.Range(XSpawnPositionRange.min, XSpawnPositionRange.max);
            spawnPosition.x = randomX;

            if (sequence.type == EnemyType.SmallGreen)
            {
                randomMultiplier = Random.Range(-1, 2);
                rotationDelay = Random.Range(0.5f, 1f);
            }

            for (int i = 0; i < sequence.count; i++)
            {
                EnemyPlane plane = ObjectPool.Instance.GetEnemyPlane(sequence.type);
                plane.SpawnAt(spawnPosition);

                if (plane.type == EnemyType.SmallGreen)
                {
                    SmallGreenEnemyPlane smallGreenEnemyPlane = plane as SmallGreenEnemyPlane;
                    smallGreenEnemyPlane.SetRotation(randomMultiplier, rotationDelay);
                }

                yield return new WaitForSeconds(sequenceInterval);
            }

            yield return new WaitForSeconds(interval);
            currentSequenceIndex++;
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