using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerupPrefabs;

    public GameObject bossPrefab;
    public GameObject[] miniEnemyPrefabs;


    private GameManager gameManager;
    public int bossRound;

    private float spawnRange = 9;

    public int enemyCount;
    public int waveNumber;

    private int difficult;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (gameManager.isGameActive)
        {
            SpawnEnemyWave(waveNumber);

            int randomPowerup = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(),
            powerupPrefabs[randomPowerup].transform.rotation);

            Debug.Log("Fail");
        }
        else return;


    }

    //limit current difficulty to be no longer than array length
    private int DifficultLevel()
    {
        if (difficult > enemyPrefab.Length)
        {
            difficult = enemyPrefab.Length;
        }
        return difficult;
    }

    //widen list of available enemies
    private int GenerateRandomEnemy()
    {
        int randomEnemy = Random.Range(0, DifficultLevel());
        return randomEnemy;
    }

    //using it to spawn an enemy
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab[GenerateRandomEnemy()], GenerateSpawnPosition(), enemyPrefab[GenerateRandomEnemy()].transform.rotation);
        }
    }

    //increase difficulty depending on the waveNumber
    void Update()
    {
        if (gameManager.isGameActive)
        {
            enemyCount = FindObjectsOfType<Enemy>().Length;

            if (enemyCount == 0)
            {
                waveNumber++;

                // Spawn Boss
                if (waveNumber % bossRound == 0)
                {
                    SpawnBossWave(waveNumber);
                }

                else
                {
                    difficult = waveNumber / 2;
                    SpawnEnemyWave(waveNumber);

                    int randomPowerup = Random.Range(0, powerupPrefabs.Length);
                    Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(),
                    powerupPrefabs[randomPowerup].transform.rotation);
                }
            }
        }

    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);


        return randomPos;
    }

    void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;

        if (bossRound != 0)
        {
            miniEnemysToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemysToSpawn = 1;
        }
        var boss = Instantiate(bossPrefab, GenerateSpawnPosition(),
        bossPrefab.transform.rotation);
        boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemysToSpawn;
    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(),
            miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }



}
