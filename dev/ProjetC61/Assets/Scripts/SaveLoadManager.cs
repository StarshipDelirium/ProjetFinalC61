using System.Collections.Generic;
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
    saveGame = JsonUtility.FromJson<SaveGame>(File.ReadAllText(Application.persistentDataPath + "/hellvaniasave.json"));
    Dictionary<string, int> Inventory = new Dictionary<string, int>();
    saveFileLoaded = true;

    Inventory.Add("HP", saveGame.HealthPotions);
    Inventory.Add("HE", saveGame.HealthElixirs);
    Inventory.Add("MP", saveGame.ManaPotions);
    Inventory.Add("ME", saveGame.ManaElixirs);
    Inventory.Add("CK", saveGame.HasCemeteryKey);

    SceneManager.LoadScene(saveGame.SceneName);
    FindObjectOfType<InventoryManager>().LoadInventory(Inventory);

    Debug.Log("LOAD: " + saveGame.ManaPotions);
  }
  public void SaveGameData(int SaveCheckpointID)
  {
    Scene scene = SceneManager.GetActiveScene();
    SaveGame saveGame = new SaveGame();
    Dictionary<string, int> playerInventory = new Dictionary<string, int>();

    saveGame.SceneName = scene.name;
    saveGame.CheckpointID = SaveCheckpointID;


    playerInventory = FindObjectOfType<InventoryManager>().GetCurrentInventory();

    foreach (KeyValuePair<string, int> item in playerInventory)
    {
      switch (item.Key)
      {
        case "HP":
          saveGame.HealthPotions = item.Value;
          break;
        case "HE":
          saveGame.HealthElixirs = item.Value;
          break;
        case "MP":
          saveGame.ManaPotions = item.Value;
          break;
        case "ME":
          saveGame.ManaElixirs = item.Value;
          break;
        case "CK":
          saveGame.HasCemeteryKey = item.Value;
          break;
        default:
          break;
      }
    }
    Debug.Log("SAVE CHECKPOINT: " + saveGame.CheckpointID);

    string jsonData = JsonUtility.ToJson(saveGame, true);                 // transforms save data into json file
    File.WriteAllText(jsonSavePath, jsonData);
  }
}
