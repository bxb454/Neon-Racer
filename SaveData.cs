using UnityEngine;
using System.IO;

/// <summary>
/// This class handles the menu screen for the Main Menu scene.
/// </summary>
public class SaveData : MonoBehaviour {

[SerializeField] private PlayerData playerData;
private string path = "";
private string persistentPath = "";

    void Start() {
        initializePlayerData();
        setPath();
        load();
    }

    ///<summary>
    ///Initialize player data.
    ///</summary>
    private void initializePlayerData() {
        playerData = new PlayerData();
        Color temp;
        if (ColorUtility.TryParseHtmlString(playerData.GetColor("color"), out temp)) {
            playerData.color = temp;
            Debug.Log("Color: " + playerData.color);
        }
        //playerData.color = playerData.GetColor("color");
        playerData.score = playerData.GetScore("score");
        
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
        //PlayerData data = new PlayerData(playerData.getScore());
        string savePath = persistentPath;
        Debug.Log("Saving to: " + savePath);
        //Debug.Log("Score: " + playerData.GetScore("score") + " Color: " + playerData.GetColor("color"));
        string json = JsonUtility.ToJson(playerData);
        Debug.Log("Saved data: " + json);
        //File.WriteAllText(savePath, json);
        using StreamWriter writer = new StreamWriter(savePath);
            writer.Write(json);
    }

    ///<summary>
    ///Load the player data from the JSON file.
    ///</summary>
    public void load() {
        string savePath = path;
        using StreamReader reader = new StreamReader(savePath);
            string json = reader.ReadToEnd();
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Loading from: " + savePath);
            Debug.Log("Loaded: " + json);
    }

    ///<summary>
    ///Update the player data.
    ///</summary>
    void Update() {
        save();
        load();
    }
}
