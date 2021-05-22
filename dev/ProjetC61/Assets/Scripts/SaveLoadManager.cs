using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{

  /********************************************************************************
   * https://docs.unity3d.com/Manual/JSONSerialization.html
   * https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
   ********************************************************************************/

  public SaveGame saveGame;
  public bool saveFileLoaded = false;
  private string jsonSavePath;


  private void Start()
  {
    jsonSavePath = Application.persistentDataPath + "/hellvaniasave.json";

    // JSON file saved under %userprofile%\AppData\LocalLow\DefaultCompany\ProjetC61

  }

  private void OnEnable()
  {
    saveGame = new SaveGame();
  }

  public void LoadGameData()
  {
    SaveGame saveGame = JsonUtility.FromJson<SaveGame>(File.ReadAllText(Application.persistentDataPath + "/hellvaniasave.json"));
    SceneManager.LoadScene(saveGame.SceneName);
    saveFileLoaded = true;
    Debug.Log("LOAD: " + saveGame.ManaPotions);
  }
  public void SaveGameData(int SaveCheckpointID)
  {
    //References
    Scene scene = SceneManager.GetActiveScene();
    SaveGame saveGame = new SaveGame();
    //Scene Name
    saveGame.SceneName = scene.name;


    saveGame.CheckpointID = SaveCheckpointID;

    saveGame.HealthPotions = GameManager.Instance.Player.redPotion;
    saveGame.ManaPotions = GameManager.Instance.Player.bluePotion;
    Debug.Log("SAVE CHECKPOINT: " + saveGame.CheckpointID);

    string jsonData = JsonUtility.ToJson(saveGame, true);                 // transforms save data into json file
    File.WriteAllText(jsonSavePath, jsonData);
  }
}
