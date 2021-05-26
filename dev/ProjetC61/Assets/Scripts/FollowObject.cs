using UnityEngine;

/************************************************************************
 * Adapté du code reçu de Maël Perreault dans le cadre du cours C63
 ************************************************************************/
public class FollowObject : MonoBehaviour
{
  public Transform TargetTransform;
  public LevelBoss LevelBoss;
  public SpriteRenderer Renderer;

  private void Start()
  {
    LevelBoss = GetComponent<LevelBoss>();
    Renderer = GetComponent<SpriteRenderer>();

    if (LevelBoss != null)
    {
      LevelBoss.OnTriggerFight += OnTriggerFight;
      LevelBoss.OnBossKilled += OnBossKilled;
    }
  }
  void LateUpdate()
  {
    if (TargetTransform != null)
    {
      // y position adjustment to avoid player being always mid screen
      float adjustedY = TargetTransform.position.y / 5;
      transform.position = new Vector3(TargetTransform.position.x, transform.position.y, transform.position.z);

    }
  }

  public void OnTriggerFight(LevelBoss levelBoss)
  {
    if (levelBoss.BossFight && !Renderer.isVisible)
    {
      TargetTransform = gameObject.transform;
      Debug.Log("BOSS FIGHT");
    }
    else
    {
      TargetTransform = GameManager.Instance.Player.transform;
    }
  }

  public void OnBossKilled(LevelBoss levelBoss)
  {
    if (levelBoss.BossKilled)
    {
      // drop item, enable door
    }
  }
}
