using UnityEngine;

/************************************************************************
 * Adapté du code reçu de Maël Perreault dans le cadre du cours C63
 ************************************************************************/
public class FollowObject : MonoBehaviour
{
  public Transform TargetTransform;
  private RestrictStage[] colliders;

  private void Start()
  {
    colliders = FindObjectsOfType<RestrictStage>();
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

  public void OnTriggerBossFight()
  {

    TargetTransform = this.transform;

    foreach (RestrictStage collider in colliders)
    {
      collider.LockStage();
    }
  }

  public void OnBossKilled()
  {
    foreach (RestrictStage collider in colliders)
    {
      collider.UnlockStage();
    }
    TargetTransform = GameManager.Instance.Player.transform;
    // drop key
  }
}
