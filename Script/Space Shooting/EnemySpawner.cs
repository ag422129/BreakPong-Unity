using UnityEngine;
using System.Collections;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemies;
        public Vector3[] spawnPoints;
        public bool isBossRound;
    }

    public Wave[] waves;

    public float spawnDelay = 0.5f;   // 敌人之间的间隔
    public float nextWaveDelay = 2f;  // 波次之间的间隔

    public SS_UIManager waveText; 

    public int currentWave = 0;
    int enemiesAlive = 0;

    public GameObject[] RockPrefabs;
    private float spawnRockInterval;
    private float minSpawnRock = 2f;
    private float maxSpawnRock = 4f;
    private bool spawningRock;
    private Coroutine rockRoutine = null;
    public bool gameStart;
    public bool Won = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameStart)
        {
            StartCoroutine(StartWaveCoroutine(0));
            gameStart = true;
        }
    }

    IEnumerator StartWaveCoroutine(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("All waves finished!");
            Won = true;
            yield break;
        }

        checkRockSpawn();
        AdjustRockInterval();


        waveText.waveNum = index + 1;
        waveText.ShowWaveText();
        waveText.WaveText.gameObject.SetActive(true);


        yield return new WaitForSeconds(nextWaveDelay);


        waveText.WaveText.gameObject.SetActive(false);

        Wave wave = waves[index];
        enemiesAlive = wave.enemies.Length;

        // 一个一个spawn敌人
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            GameObject prefab = wave.enemies[i];
            Vector3 pos = wave.spawnPoints[i];

            GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);

            Enemy4 En4 = enemy.GetComponent<Enemy4>();
            if (En4 != null)
            {
                En4.isBoss = wave.isBossRound;
            }

            Enemy e = enemy.GetComponent<Enemy>();
            e.onDeath += OnEnemyKilled;

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            currentWave++;
            StartCoroutine(StartWaveCoroutine(currentWave));
        }
    }

    void checkRockSpawn()
    {
        if(currentWave >= 4)
        {
            if (!spawningRock)
            {
                spawningRock = true;
                rockRoutine = StartCoroutine(RockSpawner());
            }
        }
        else
        {
            if (spawningRock)
            {
                spawningRock = false;
                StopCoroutine(rockRoutine);
            }
        }
    }
    void SpawnRock()
    {
        int Rock = Random.Range(0, RockPrefabs.Length);

        Vector3 RockPos = new Vector3(Random.Range(-10.5f, 10.5f), 6f, 0);

        GameObject rocks = Instantiate(RockPrefabs[Rock], RockPos, Quaternion.identity);

        Rock r = rocks.GetComponent<Rock>();
        if(r != null)
        {
            r.speed += currentWave * 0.5f;
        }
    }

    IEnumerator RockSpawner()
    {
        while (spawningRock)
        {
            SpawnRock();
            spawnRockInterval = Random.Range(minSpawnRock, maxSpawnRock);
            yield return new WaitForSeconds(spawnRockInterval);
        }
        yield return null;
    }

    void AdjustRockInterval()
    {
        if(currentWave >= 4)
        {
            minSpawnRock -= 0.1f;
            maxSpawnRock -= 0.1f;

            minSpawnRock = Mathf.Max(minSpawnRock, 0.5f);
            maxSpawnRock = Mathf.Max(maxSpawnRock, 2f);
        }
    }
}
