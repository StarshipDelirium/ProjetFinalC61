using System;

[Serializable]
public class SaveGame
{
  public string SceneName = "";
  public int CheckpointID = 0;                              // ID of save checkpoint statue?
  public int HealthPotions = 0;
  public int ManaPotions = 0;
  public bool HasCemeteryKey = false;



}
