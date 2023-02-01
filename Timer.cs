using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour {

    public float timeLeft {get; set;}
    public float timeElapsed {get; set;}

    public float elapseTimer() {
        timeElapsed += Time.deltaTime;
        float seconds = (Time.deltaTime) % 60;
        return seconds * 1.0f;
    }

    public void decrementTimeLeft() {
        if (timeLeft > 0) {
        timeLeft -= elapseTimer();
        }
    }

    public void addTime(float time) {
        this.timeLeft += time;
    }

    // Start is called before the first frame update
    void Start() {
        timeLeft = 100.0f;
        timeElapsed = 0.0f;
    }

    // Update is called once per frame
    void Update() {
            decrementTimeLeft();
    }
}
