using UnityEngine;

public class Item : MonoBehaviour
{
  public string Name;                   // item name to be displayed in menu
  public string Description;
  public string ID;                     // if item with same ID already in inventory, increment total count instead of using another slot
  public int Stats;                     // item effects, hp or mp
  public int TotalCount;
  public bool IsConsumable;
  public virtual void Use() { }
  public void CheckCount()
  {
    if (TotalCount == 0)
    {
      FindObjectOfType<InventoryManager>().RemoveItem(ID);
    }
  }
}
