using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*************************************************************************************************
 * https://www.geeksforgeeks.org/c-sharp-dictionary-with-examples/
 ************************************************************************************************/
public class InventoryManager : MonoBehaviour
{
  public Animator Animator;
  public InventorySlot[] Slots;
  public Text ItemName;
  public Text ItemDescription;
  public Button UseButton;
  public RectTransform Panel;
  Sprite HealthPotion, HealthElixir, ManaPotion, ManaElixir, CemeteryKey;
  private bool onScreen = false;
  private Item selectedItem;
  void Start()
  {
    HealthPotion = Resources.Load<Sprite>("items/health_potion");
    HealthElixir = Resources.Load<Sprite>("items/health_elixir");
    ManaPotion = Resources.Load<Sprite>("items/mana_potion");
    ManaElixir = Resources.Load<Sprite>("items/mana_elixir");
    CemeteryKey = Resources.Load<Sprite>("items/CemeteryKey");
    Slots = Panel.GetComponentsInChildren<InventorySlot>();
  }

  private void OnEnable()
  {
    Button button = UseButton.GetComponent<Button>();
    button.onClick.AddListener(UseItem);
    ClearItemInfo();                                         // clear last item selection info + only show Use button if consumable items
  }
  public void CheckInventory(string newItemID)
  {
    bool hasThisItem = false;
    foreach (InventorySlot slot in Slots)
    {

      if (slot.hasItem)
      {
        var slotItem = slot.GetComponent<Item>();

        if (newItemID.Equals(slotItem.ID))
        {
          ++slotItem.TotalCount;                        // increment same item count if already in inventory
          slot.Qty.text = slotItem.TotalCount.ToString();
          hasThisItem = true;
          Debug.Log(slotItem.Name + " " + slotItem.TotalCount);
        }
      }
    }

    if (!hasThisItem)
    {
      AddItem(newItemID);                                 // or add to inventory
    }

  }

  public void AddItem(string newItemID, int optionalSaveQty = 0)                      // optional int qty to use same function when finding new items in game or loading a save file
  {
    foreach (InventorySlot slot in Slots)
    {

      if (!slot.hasItem)
      {
        switch (newItemID)
        {
          case "HP":                                              // Health Potion
            slot.gameObject.AddComponent<HealthPotion>();
            slot.GetComponent<InventorySlot>().Icon.sprite = HealthPotion;
            break;
          case "HE":                                              // Health Elixir
            slot.gameObject.AddComponent<HealthElixir>();
            slot.GetComponent<InventorySlot>().Icon.sprite = HealthElixir;
            break;
          case "MP":                                              // Mana Potion
            slot.gameObject.AddComponent<ManaPotion>();
            slot.GetComponent<InventorySlot>().Icon.sprite = ManaPotion;
            break;
          case "ME":                                              // Mana Elixir
            slot.gameObject.AddComponent<ManaElixir>();
            slot.GetComponent<InventorySlot>().Icon.sprite = ManaElixir;
            break;
          case "CK":                                              // Cemetery Key
            slot.gameObject.AddComponent<CemeteryKey>();
            slot.GetComponent<InventorySlot>().Icon.sprite = CemeteryKey;
            break;
          default:
            break;

        }

        var tempColor = slot.Icon.color;
        tempColor.a = 1f;
        slot.Icon.color = tempColor;
        slot.hasItem = true;
        slot.GetComponent<Button>().interactable = true;

        Item item = slot.GetComponent<Item>();
        if (optionalSaveQty > 0)
        {
          item.TotalCount = optionalSaveQty;
        }
        else
        {
          item.TotalCount = 1;
        }

        slot.Qty.text = item.TotalCount.ToString();
        Debug.Log(item.Name + " " + item.TotalCount);
        break;
      }
    }
  }

  public void RemoveItem(string itemID)
  {
    foreach (InventorySlot slot in Slots)
    {
      if (slot.hasItem)
      {
        var slotItem = slot.GetComponent<Item>();
        if (itemID.Equals(slotItem.ID))
        {
          switch (itemID)
          {
            case "HP":
              HealthPotion hp = slot.gameObject.GetComponent<HealthPotion>();
              Destroy(hp);                                                                  // Removes component without destroying InventorySlot object
              break;
            case "HE":
              HealthElixir he = slot.gameObject.GetComponent<HealthElixir>();
              Destroy(he);
              break;
            case "MP":
              ManaPotion mp = slot.gameObject.GetComponent<ManaPotion>();
              Destroy(mp);
              break;
            case "ME":
              ManaElixir me = slot.gameObject.GetComponent<ManaElixir>();
              Destroy(me);
              break;
            case "CK":
              CemeteryKey ck = slot.gameObject.GetComponent<CemeteryKey>();
              Destroy(ck);
              break;
            default:
              break;
          }

          slot.hasItem = false;
          slot.Icon.sprite = null;
          slot.Qty.text = "";
          slot.GetComponent<Button>().interactable = false;
          ClearItemInfo();

          var tempColor = slot.Icon.color;                                              // set alpha to 0 to avoid white image over button
          tempColor.a = 0f;
          slot.Icon.color = tempColor;

          break;
        }
      }
    }
  }
  public void OpenInventory()
  {
    if (!onScreen)
    {
      GameManager.Instance.PauseGame();
      Animator.SetBool("isOpen", true);
      onScreen = true;
    }
    else
    {
      CloseInventory();
    }

  }

  public void CloseInventory()
  {
    Animator.SetBool("isOpen", false);
    onScreen = false;
    GameManager.Instance.ResumeGame();

  }

  public void DisplayItemInfo(Item item)
  {
    selectedItem = item;
    ItemName.text = item.Name;
    ItemDescription.text = item.Description;

    if (item.IsConsumable)
    {
      UseButton.gameObject.SetActive(true);
    }
    else
    {
      UseButton.gameObject.SetActive(false);
    }

  }

  private void ClearItemInfo()
  {
    UseButton.gameObject.SetActive(false);
    ItemName.text = "";
    ItemDescription.text = "";
  }

  private void ClearInventory()                                                   // Remove all items before loading save file to ensure correct items and count
  {
    string[] itemIDs = { "HP", "HE", "MP", "ME", "CK" };

    foreach (string id in itemIDs)
    {
      RemoveItem(id);
    }
  }

  private void UseItem()
  {
    if (onScreen)
    {
      GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Select);
      selectedItem.Use();
    }
  }

  public void LoadInventory(Dictionary<string, int> inventory)                      // load inventory from last save
  {
    ClearInventory();                                                               // clear first to avoid duplicates and wrong qty

    foreach (KeyValuePair<string, int> item in inventory)
    {
      if (item.Value > 0)                                                           // only display items on hand
      {
        AddItem(item.Key, item.Value);
      }
    }
  }

  public Dictionary<string, int> GetCurrentInventory()
  {
    Dictionary<string, int> currentInventory = new Dictionary<string, int>();                 // create Dictionary of itemID and quantity on hand for save file

    Debug.Log("SLOTS: " + Slots.ToString());

    foreach (InventorySlot slot in Slots)
    {
      if (slot.hasItem)
      {
        Item item = slot.GetComponent<Item>();

        string itemID = item.ID;
        int qty = item.TotalCount;
        Debug.Log("ITEM " + itemID + " " + qty);

        currentInventory.Add(itemID, qty);
      }
    }

    return currentInventory;
  }
}
