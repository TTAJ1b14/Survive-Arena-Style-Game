using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject powerUpPrefab;

    private float spawnRange = 9;

    public int enemyCount;
    public int waveNumber;

    private int difficult;


    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
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
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            difficult = waveNumber / 2;
            SpawnEnemyWave(waveNumber);
            Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);


        return randomPos;
    }


}
