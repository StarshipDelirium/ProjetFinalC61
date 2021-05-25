using UnityEngine;

public class LevelBoss : MonoBehaviour
{
  public delegate void LevelBossEvent(LevelBoss levelBoss);

  public LevelBossEvent OnTriggerFight;
  public LevelBossEvent OnBossKilled;

  private bool _bossFight;
  private bool _bossKilled;

  public bool BossFight
  {
    get { return _bossFight; }
    set
    {
      var previous = _bossFight;

      if (_bossFight != previous)
      {
        OnTriggerFight?.Invoke(this);

      }
    }
  }

  public bool BossKilled
  {
    get { return _bossKilled; }
    set
    {
      var previous = _bossKilled;

      if (_bossKilled != previous)
      {
        OnBossKilled?.Invoke(this);
      }
    }
  }

  private void OnEnable()
  {
    _bossFight = _bossKilled = false;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
