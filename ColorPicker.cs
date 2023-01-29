using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to control the color of the vehicle.
/// </summary>
public class ColorPicker : MonoBehaviour {
    [SerializeField] private FlexibleColorPicker colorPicker;
    public Material mat;
    private SaveData saveData;
    private Color color;
    private PlayerData playerData;

    /// <summary>
    /// This method is used to initialize the color of the vehicle.
    /// </summary>
    void Start() {
       colorPicker.SetColor(saveData.data.color);
       Debug.Log("Color: " + saveData.data.color);
       color = mat.color;
    }

    /// <summary>
    /// This method is used to update the color of the vehicle.
    /// </summary>
    void Update() {
        mat.color = colorPicker.color;
        color = mat.color;
    }
}
