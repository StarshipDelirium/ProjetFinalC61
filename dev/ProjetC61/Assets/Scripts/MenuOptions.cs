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
    Debug.Log(selection);

    if (selection.Equals("Start"))
    {

      transition.FadeToLevel(1);
      //SceneManager.LoadScene(1);
    }
    else if (selection.Equals("Quit"))
    {
      Application.Quit();
    }
  }

}
