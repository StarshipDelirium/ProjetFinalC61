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
  private Color defaultColor;
  private Font defaultFont;
  private int defaultSize;
  private Color selectedColor;
  private Font SelectedFont;
  public int selectedSize;

  private void Awake()
  {
    TextLabel = gameObject.GetComponent<Text>();
    defaultFont = TextLabel.font;
    defaultColor = TextLabel.color;
    defaultSize = TextLabel.fontSize;
    selectedColor = Color.red;
    SelectedFont = (Font)Resources.Load("Fonts/GhastlyPixe");
  }
  public void OnPointerEnter(PointerEventData eventData)
  {
    TextLabel.font = SelectedFont;
    TextLabel.color = selectedColor;
    TextLabel.fontSize = selectedSize;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    TextLabel.font = defaultFont;
    TextLabel.color = defaultColor;
    TextLabel.fontSize = defaultSize;
  }
}
