using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuOptions : MonoBehaviour, IPointerClickHandler
{
  LevelTransition transition;

  private void Awake()
  {
    transition = FindObjectOfType<LevelTransition>();
  }
  public void OnPointerClick(PointerEventData eventData)
  {
    var selection = eventData.rawPointerPress.name;
    GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Select);

    if (selection.Equals("Start"))
    {
      transition.FadeToLevel(1);

      if (File.Exists(Application.persistentDataPath + "/hellvaniasave.json"))                                     // if new game selected and a save file exists, delete save file
      {
        File.Delete(Application.persistentDataPath + "/hellvaniasave.json");
      }
    }
    else if (selection.Equals("Load"))
    {
      FindObjectOfType<SaveLoadManager>().LoadGameData();
    }
    else if (selection.Equals("Quit"))
    {
      Application.Quit();
    }
  }

}
