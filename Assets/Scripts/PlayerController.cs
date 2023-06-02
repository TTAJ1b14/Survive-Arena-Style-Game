using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody playeRb;
    private GameObject focalPoint;
    public GameObject powerUpIndicator;
    public bool hasPowerup;
    private float powerUpStrength = 5000f;

    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;
    // Start is called before the first frame update
    void Start()
    {
        playeRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playeRb.AddForce(focalPoint.transform.forward * speed * verticalInput);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }
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
