using UnityEngine;

public class PickUp : MonoBehaviour
{
  private string itemType;
  private void Awake()
  {
    itemType = gameObject.tag;
  }
  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (GameManager.Instance.Player != null)
    {
      FindObjectOfType<InventoryManager>().CheckInventory(itemType);               // add item to inventory or increment total count if already on hand
      Destroy(gameObject);
    }
  }
}
