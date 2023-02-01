using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used to control the garage scene.
/// </summary>
public class GarageMenu : MonoBehaviour {
    [SerializeField] private Camera garageCam;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 prevPos;
    [SerializeField] private float disOff;
    [SerializeField] private Vector3 offset = new Vector3(0, 0.0f, -21.4f);
    [SerializeField] private Button returnMenuButton;
    [SerializeField] private Image screenFader;

    /// <summary>
    /// This function is used to create a fade in effect when the scene loads.
    /// </summary>
    void Start() {
        LeanTween.reset();
         screenFader.gameObject.SetActive(true);
        LeanTween.alpha(screenFader.rectTransform, 0, 1.1f).setOnComplete(() => {
            screenFader.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// This function is used to move the camera to the target object.
    /// </summary>
    void Update() {
        //This is used to move the camera to the target object.
        var inputDown = SimpleInput.GetMouseButtonDown(0);
        var inputReleased = SimpleInput.GetMouseButton(0);

        //If user is dragging finger on the screen, move the camera.
        if(inputDown) {
            prevPos = garageCam.ScreenToViewportPoint(Input.mousePosition) + new Vector3(0, 0, 0);
        //Or else, if the user is not dragging the finger, then move the camera to the target object.
        }
        else if (inputReleased) {
            Vector3 newPos = garageCam.ScreenToViewportPoint(Input.mousePosition) + new Vector3(0, 0, 100);
            Vector3 dir = prevPos - newPos;

            //Rotate the camera based on the mouse movement.
            float rotY = -dir.x * 180 * mouseSensitivity;
            float rotX = -dir.y * 180 * mouseSensitivity;

            //Clamp the rotation so that the camera does not rotate too far.
            var fixedRotX = Mathf.Clamp(rotX, -0.002f, 0.003f);
            var fixedRotY = Mathf.Clamp(rotY, -80, 80);

            //Move the camera to the target object.
            garageCam.transform.position = target.position + new Vector3(-1.3f, 4.55f, 0);
            garageCam.transform.Rotate(Vector3.right, fixedRotX);
            garageCam.transform.Rotate(Vector3.up, fixedRotY, Space.World);
            garageCam.transform.Translate(new Vector3(0, 0, -disOff));

            //Set the previous position to the new position.
            prevPos = newPos;
        }
    }

    /// <summary>
    /// This function is used to return to the main menu, and save the player's picked color.
    /// </summary>
    public void onSaveClick() {
       screenFader.gameObject.SetActive(true);
        LeanTween.alpha(screenFader.rectTransform, 0, 0f);
        LeanTween.alpha(screenFader.rectTransform, 1, 1.1f).setOnComplete(() => {
            SceneManager.LoadScene("MainMenu");
    });
    }
}
