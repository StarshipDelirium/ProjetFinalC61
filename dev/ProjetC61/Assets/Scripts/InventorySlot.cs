using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
  public bool hasItem;
  public Image Icon;
  public Text Qty;
  public Button Button;

  private void Awake()
  {
    Button button = Button.GetComponent<Button>();
    button.onClick.AddListener(OnSlotClicked);
  }

  private void OnSlotClicked()
  {
    Item item = gameObject.GetComponent<Item>();

    if (item != null)
    {
      GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Select);
      FindObjectOfType<InventoryManager>().DisplayItemInfo(item);
    }

  }
}
