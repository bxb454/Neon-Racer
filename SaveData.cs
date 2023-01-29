using UnityEngine;
using System.IO;
using System.Text;
using System;

/// <summary>
/// This class is used to save and load data to a JSON file with Base64 encoding and decoding to prevent the file from being easily
/// readable by the user.
/// </summary>
public class SaveData : MonoBehaviour {
private string path = "";
private string persistentPath = "";

/// <summary>
///Nested data type that acts as the 'type' of data to be stored in the JSON file.
[System.Serializable]
public class Data {
    public float score;
    public Material material;
    public Color color;
}

//Declare data type
public Data data = new Data();

    ///<summary>
    ///Method that is called when the script is loaded.
    ///</summary>
    void Start() {
        setPath();
        load();
    }

    ///<summary>
    ///Set the path to the JSON file.
    ///</summary>
    private void setPath() {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "playerData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "playerData.json";
    }

    ///<summary>
    ///Save the player data to the JSON file.
    ///</summary>
    public void save() {
        string savePath = persistentPath;
        Debug.Log("Saving to: " + savePath);
        string json = JsonUtility.ToJson(data);
        Debug.Log("Saved data: " + json);
        //File.WriteAllText(savePath, json);
        File.WriteAllText(savePath, Convert.ToBase64String(Encoding.UTF8.GetBytes(json)));
    }

    ///<summary>
    ///Load the player data from the JSON file.
    ///</summary>
    public void load() {
        string savePath = persistentPath;
        Debug.Log("Loading from: " + savePath);
       string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<Data>(Encoding.UTF8.GetString(Convert.FromBase64String(json)));
            Debug.Log("Loaded: " + json);
    }

    ///<summary>
    ///Update the player data.
    ///</summary>
    void Update() {
        //Set color to be the same as the material color.
        data.color = data.material.color;
        save();
    }
}
