using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class handles the menu screen for the Main Menu scene.
/// </summary>
public class MenuScreen : MonoBehaviour {
    [SerializeField] private Image screenFader;
    // Start is called before the first frame update
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button garageButton;

    /// <summary>
    /// When the start button is clicked, fade the screen and load the main game scene.
    /// </summary>
    public void onStartButtonClick() {
        screenFader.gameObject.SetActive(true);
        LeanTween.alpha(screenFader.rectTransform, 0, 0f);
        LeanTween.alpha(screenFader.rectTransform, 1, 1.1f).setOnComplete(() => {
            SceneManager.LoadScene("MainGame");
        });
        Debug.Log("Start button clicked");
    }

    /// <summary>
    /// When the quit button is clicked, quit the application.
    /// </summary>
    public void onQuitButtonClick() {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }

    /// <summary>
    /// When the options button is clicked, load the options scene.
    /// </summary>
    public void onOptionsButtonClick() {
        Debug.Log("Options button clicked");
        //SceneManager.LoadScene("Options");
    }

    /// <summary>
    /// When the garage button is clicked, load the garage scene.
    /// </summary>
    public void onGarageButtonClick() {
        Debug.Log("Garage button clicked");
        SceneManager.LoadScene("Garage");
    }

    /// <summary>
    ///Reset LeanTween in order to prevent errors in screen fade effects.
    /// </summary>
    void Start() {
        LeanTween.reset();
    }
}
