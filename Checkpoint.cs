using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Checkpoint : MonoBehaviour {

[SerializeField] private float timeAdd;
[SerializeField] private Timer timer;
private bool isConsumed = false;

    /// <summary>
    /// When the player enters the trigger, add time to the timer.
    /// </summary>
    void OnTriggerEnter(Collider other) {
        timer.timeLeft += timeAdd;
        isConsumed = true;
        if(isConsumed) {
            return;
        }
    }
}
