using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    public TextMeshProUGUI CounterText;
    bool countActive = true;

    private int Count = 0;

    private void Start()
    {
        Count = 0;
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
        }

    }
}
