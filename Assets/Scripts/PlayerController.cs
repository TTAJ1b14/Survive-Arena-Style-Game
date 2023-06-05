using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody playerRb;
    private GameObject focalPoint;

    // General PowerUp Variables
    public GameObject powerUpIndicator;
    public bool hasPowerup;

    // Pushback PowerUp Variables
    private float powerUpStrength = 5000f;

    // Rockets PowerUp Variables
    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

    // Smash PowerUp Variables
    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;

    bool smashing = false;
    float floorY;



    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * verticalInput);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchRockets();
        }

        if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
        }
    }

    IEnumerator Smash()
    {
        // Find Regular Enemies
        var enemies = FindObjectsOfType<Enemy>();
        //Store the y position before taking off
        floorY = transform.position.y;
        //Calculate the amount of time we will go up
        float jumpTime = Time.time + hangTime;
        Debug.Log(" Smash");
        while (Time.time < jumpTime)
        {
            //move the player up while still keeping his x velocity.
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            Debug.Log(" Smash: Player move up");
            yield return null;
        }
        //Now move the player down
        while (transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            Debug.Log(" Smash: Player moved down");
            yield return null;
        }
        //Cycle through all enemies.
        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position.
            if (enemies[i] != null)
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
                Debug.Log(" Smash: Found Enemy");
        }
        playerRb.velocity = Vector3.zero;
        Debug.Log(" Smash: Set flying to Zero");
        //We are no longer smashing, so set the boolean to false
        smashing = false;
    }

    // Destroy Power Up when collected
    // Set powerUp bool true
    // Start Coroutine to deactivate powerUp
    // Activate PowerUp indicator ring
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {

            hasPowerup = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;

            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerUpCountDownRoutine());

            powerUpIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    // 7 seconds Timer
    // Turn off powerup after 7 seconds
    // Turn off powerup indicator ring
    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerUpIndicator.gameObject.SetActive(false);
        currentPowerUp = PowerUpType.None;
    }

    // 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);

            Debug.Log("Player collided with: " + collision.gameObject.name + " with powerup set to " + currentPowerUp.ToString());
        }
    }

    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up,
            Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }
}
