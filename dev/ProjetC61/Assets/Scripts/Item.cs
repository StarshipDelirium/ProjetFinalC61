using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
  public string Name;
  public string Description;
  public Image Icon;
  public int TotalCount;
  public virtual void Use() { }
}
