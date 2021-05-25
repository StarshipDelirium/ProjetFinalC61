using System;

[Serializable]
public class SaveGame
{
  public string SceneName = "";
  public int CheckpointID = 0;                              // ID of save checkpoint statue?
  public int HealthPotions = 0;
  public int HealthElixirs = 0;
  public int ManaPotions = 0;
  public int ManaElixirs = 0;
  public int HasCemeteryKey = 0;                            // int instead of bool to include it in a Dictionary<string,int>
  public int CurrentHP = 0;
  public int CurrentMana = 0;




}
