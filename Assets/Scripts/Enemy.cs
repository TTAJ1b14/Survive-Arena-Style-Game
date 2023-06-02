using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody enemyRb;
    public float speed;
    private GameObject player;

    private SpawnManager spawnManagerScript;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player"); 

        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
         if (transform.position.y >-1)
        {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * (speed + spawnManagerScript.waveNumber * 5));
        }


        if (transform.position.y < -15)
        {
            Destroy(gameObject);
    
        }
    }
}
