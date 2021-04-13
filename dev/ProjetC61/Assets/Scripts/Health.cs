using UnityEngine;

public class Health : MonoBehaviour
{
  public delegate void HealthEvent(Health health);

  public HealthEvent OnChanged;
  public HealthEvent OnHit;
  public HealthEvent OnDeath;

  public int Max;
  public int Start;
  private int _value;

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

        if (_value < previous)
        {
          OnHit?.Invoke(this);
        }

        if (_value <= 0)
        {
          OnDeath?.Invoke(this);
        }
      }
    }
  }

  private void Awake()
  {
    _value = Start;
  }
}
