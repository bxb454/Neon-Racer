using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerGUI : MonoBehaviour {
    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject vehicle;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button returnToMenuButton;

    ///<summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start() {
        timerText.text = timer.timeLeft.ToString("F1");
        gameOverText.gameObject.SetActive(false);
    }

    ///<summary>
    ///Update is called once per frame
    ///</summary>
    void Update() {
        timerText.text = timer.timeLeft.ToString("F1");
        if(timer.timeLeft <= 0) {
            GameObject.Destroy(vehicle);
            gameOverText.gameObject.SetActive(true);
            returnToMenuButton.gameObject.SetActive(true);
        }
    }

    ///<summary>
    ///When the return to menu button is clicked, load the menu scene.
    ///</summary>
    public void onReturnToMenuButtonClick() {
        SceneManager.LoadScene("MainMenu");
    }
}