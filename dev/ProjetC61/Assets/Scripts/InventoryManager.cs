using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryManager : MonoBehaviour
{
  public Animator Animator;
  public Button[] Slots;                            // disabled when empty inventory to detect with slots are empty
  private bool onScreen = false;
  void Start()
  {
  }
  void Update()
  {

  }

  public void CheckInventory(string itemName)
  {

    foreach (Button slot in Slots)
    {


    }

    AddItem(itemName);
  }

  public void AddItem(string itemName)
  {

  }

  public bool RemoveItem(string itemName)
  {
    //
    return true;
  }

  public int GetItemCount(string itemName)
  {
    return 0;
  }

  public void OpenInventory()
  {
    if (!onScreen)
    {
      Animator.SetBool("isOpen", true);
      Debug.Log("INVENTORY OPEN");
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
    Debug.Log("INVENTORY CLOSED");

  }

  public void TestAdd()
  {
    foreach (Button slot in Slots)
    {


      //slot.GetComponentInChildren<Image>().canvas. = Resources.Load("items/health_elixir.png") as Sprite;


      var tempColor = slot.GetComponentInChildren<Image>().color;
      tempColor.a = 1f;
      slot.GetComponentInChildren<Image>().color = tempColor;
      slot.interactable = true;

    }
    //Name = "Health Elixir";
    //Description = "Fully restores health";

  }


}
