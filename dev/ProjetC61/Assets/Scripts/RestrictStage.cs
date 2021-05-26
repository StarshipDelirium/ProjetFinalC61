using UnityEngine;

public class RestrictStage : MonoBehaviour
{
  public BoxCollider2D StageCollider;
  void Start()
  {
    StageCollider = GetComponent<BoxCollider2D>();
    StageCollider.enabled = false;
  }

  public void LockStage()
  {
    StageCollider.enabled = true;                                           // colliders enabled to lock player in boss battle area
  }

  public void UnlockStage()
  {
    StageCollider.enabled = false;
  }
}
