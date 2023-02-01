using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class is used to display the distance and speed of the vehicle.
/// </summary>
public class DistanceGUI : MonoBehaviour {
    [SerializeField] private Transform vehicle;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float negativeDistance;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private string distanceMessage;
    [SerializeField] private string speedMessage;
    [SerializeField] private SaveData saveData;
    [SerializeField] private TextMeshProUGUI scoreText;
    private const float convertMetersPerSecondToMilesPerHour = 2.237f;
    private const float scoreMultiplier = 0.05f;
    
    /// <summary>
    /// This method is used to initialize the distance and speed of the vehicle.
    /// </summary>
    void Start() {
        distanceText.text = distanceMessage + (Mathf.Abs(vehicle.transform.position.z - Mathf.Abs(negativeDistance))).ToString("F2") + " m";
        speedText.text = speedMessage + (convertMetersPerSecondToMilesPerHour * Mathf.Abs(rb.velocity.z)).ToString("F1") + " mph";
        scoreText.text = "Score: " + saveData.data.score.ToString();
    }

    /// <summary>
    /// This method is used to update the distance and speed of the vehicle, and accordingly updating player score by distance travelled.
    /// </summary>
    void Update() {
        if(Time.timeScale != 0) {
        var vehicleSpeed = (Mathf.Abs(vehicle.transform.position.z - Mathf.Abs(negativeDistance)));
        distanceText.text = distanceMessage + vehicleSpeed.ToString("F2") + " m";
        speedText.text = speedMessage + (convertMetersPerSecondToMilesPerHour * Mathf.Abs(rb.velocity.z)).ToString("F1") + " mph";
        if(rb.velocity.z < -0.02f) {
        saveData.data.score += (int)(Mathf.Abs(vehicle.transform.position.z - Mathf.Abs(negativeDistance))* scoreMultiplier);
        }
        scoreText.text = "Score: " + saveData.data.score.ToString();
        }
    }
}
