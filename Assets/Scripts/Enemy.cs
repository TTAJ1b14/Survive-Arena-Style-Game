using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody enemyRb;
    public float speed;
    private GameObject player;

    private SpawnManager spawnManagerScript;
    private GameManager gameManager;

    // Boss 
    public bool isBoss = false;
    public float spawnInterval;
    float nextSpawn;
    public int miniEnemySpawnCount;



    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        if (isBoss)
        {
            spawnManagerScript = FindObjectOfType<SpawnManager>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            if (transform.position.y > -1)
            {
                Vector3 lookDirection = (player.transform.position - transform.position).normalized;
                enemyRb.AddForce(lookDirection * speed);
            }
            if (isBoss)
            {
                if (Time.time > nextSpawn)
                {
                    nextSpawn = Time.time + spawnInterval;
                    spawnManagerScript.SpawnMiniEnemy(miniEnemySpawnCount);
                }
            }

            if (transform.position.y < -15)
            {
                Destroy(gameObject);
            }
        }
        else return;
    }
}
