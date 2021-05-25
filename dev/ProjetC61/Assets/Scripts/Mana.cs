using UnityEngine;

public class Mana : MonoBehaviour
{
  public int Max = 10;
  public int StartValue = 10;

  public delegate void ManaEvent(Mana mana);

  public ManaEvent OnChanged;
  public ManaEvent OnUse;

  private int _value;

  private void Awake()
  {
    if (GameManager.Instance.SaveLoaded)
    {
      _value = GameManager.Instance.PlayerMana;
    }
    else
    {
      _value = Max;
    }
  }

  public int Value
  {
    get { return _value; }
    set
    {
      var previous = _value;

      _value = Mathf.Clamp(value, 0, Max);

      if (_value != previous)
      {
        OnChanged?.Invoke(this);
      }
    }
  }
}
