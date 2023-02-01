using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the pause menu.
/// </summary>
public class OnButtonClick : MonoBehaviour {
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;

    [SerializeField] private Image screenFader;

    public void onClick() {
        Time.timeScale = 0;
        mainMenuButton.gameObject.SetActive(false);
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
    }

    public void onQuitClick() {
        Application.Quit();
    }

    public void onResumeClick() {
        Time.timeScale = 1;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(true);
    }

    public void onMainMenuClick() {
        SceneManager.LoadScene("MainMenu");
    }

    void Start() {
        LeanTween.reset();
         screenFader.gameObject.SetActive(true);
        LeanTween.alpha(screenFader.rectTransform, 0, 1.1f).setOnComplete(() => {
            screenFader.gameObject.SetActive(false);
        });
    }
}
