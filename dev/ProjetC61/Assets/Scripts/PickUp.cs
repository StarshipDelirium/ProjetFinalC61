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

      switch (itemType)                                    // depending on which item type pickup script is attached to
      {
        case "HealthPotion":
          break;
        case "HealthElixir":
          break;
        case "ManaPotion":
          break;
        case "ManaElixir":
          break;
      }
    }
  }
}
