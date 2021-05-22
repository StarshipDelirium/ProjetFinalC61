using UnityEngine;


public class GameProgress : MonoBehaviour
{
  public enum EquipedHealth                 // if player has potions, will be displayed in UI
  {
    RedPotion,

    None
  }

  public enum EquipedMana
  {
    BluePotion,

    None
  }

  public SaveCheckpoint SaveCheckpoint;
  public string playerName = "";
  public int LastCheckpointSave = 0;

  // Cemetery Level
  public bool hasReachedCrypt = false;
  public bool hasKilled;
  public bool hasCryptKey = false;



  public bool hasMasterSword = false;
  public bool hasBoomerang = false;
  public bool hasFireball = false;
  public bool hasFrostbolt = false;
  public bool hasBomb = false;
  public EquipedHealth equipedHealth = EquipedHealth.None;
  public EquipedMana equipedMana = EquipedMana.None;

  private void Awake()
  {
    SaveCheckpoint = GetComponent<SaveCheckpoint>();
    SaveCheckpoint.OnChanged += OnChanged;
  }

  private void OnChanged(SaveCheckpoint checkpoint)
  {
    LastCheckpointSave = checkpoint.Value;              // updated each time player saves
  }
}
