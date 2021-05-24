using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
  public bool hasItem;
  public Image Icon;
  public Text Qty;
  public Button Button;

  private void Start()
  {
    Button button = Button.GetComponent<Button>();
    button.onClick.AddListener(OnSlotClicked);
  }

  private void OnSlotClicked()
  {
    Item item = GetComponent<Item>();

    if (item != null)
    {
      Debug.Log(item.Name + " clicked");
      FindObjectOfType<InventoryManager>().DisplayItemInfo(item);
    }

  }
}
