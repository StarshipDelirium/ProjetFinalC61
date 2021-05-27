using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*****************************************************************************
 * With help from Unity Forums: 
 * https://answers.unity.com/questions/1199251/onmouseover-ui-button-c.html *

 ******************************************************************************/
public class FontBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  public Text TextLabel;
  public int selectedSize;
  public Animator Animator;
  private Color defaultColor;
  private Font defaultFont;
  private int defaultSize;
  private Color selectedColor;
  private Font SelectedFont;
  private bool isDisabled = false;
  private bool isShowingControls = false;

  private void Start()
  {
    TextLabel = gameObject.GetComponent<Text>();
    defaultFont = TextLabel.font;
    defaultColor = TextLabel.color;
    defaultSize = TextLabel.fontSize;
    selectedColor = Color.red;
    SelectedFont = (Font)Resources.Load("Fonts/GhastlyPixe");

    if (!File.Exists(Application.persistentDataPath + "/hellvaniasave.json"))                                         // if no save file detected, change LoadGame color to grey
    {
      if (TextLabel.CompareTag("LoadGame"))
      {
        defaultColor = Color.grey;
        TextLabel.color = defaultColor;
        isDisabled = true;
      }
    }
  }
  public void OnPointerEnter(PointerEventData eventData)
  {

    if (!isDisabled)
    {
      TextLabel.font = SelectedFont;
      TextLabel.color = selectedColor;
      TextLabel.fontSize = selectedSize;
      GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Hover);
    }
    else if (!gameObject.CompareTag("LoadGame"))                                                     // if no save file, only apply OnHover mouse behaviour to non Load options
    {
      TextLabel.font = SelectedFont;
      TextLabel.color = selectedColor;
      TextLabel.fontSize = selectedSize;
    }

    if (gameObject.CompareTag("Controls"))
    {
      Animator.SetBool("isMenuOpen", true);
      isShowingControls = true;
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    TextLabel.font = defaultFont;
    TextLabel.color = defaultColor;
    TextLabel.fontSize = defaultSize;

    if (isShowingControls && gameObject.CompareTag("Controls"))
    {
      Animator.SetBool("isMenuOpen", false);
      isShowingControls = false;
    }
  }
}
