using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public bool isGameActive = false;
    public GameObject titleScreen;

    public TextMeshProUGUI CounterText;
    bool countActive = true;

    private int Count = 0;


    public Button restartButton;
    public Button startButton;

    private void Start()
    {
        Count = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        isGameActive = true;
        titleScreen.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && countActive)
        {
            Count += 1;
            CounterText.text = "Balls Dodged:" + Count;
        }

        if (other.CompareTag("Player"))
        {
            CounterText.text = "Player Fell :(";
            countActive = false;
            restartButton.gameObject.SetActive(true);
            isGameActive = false;
        }

    }



}
