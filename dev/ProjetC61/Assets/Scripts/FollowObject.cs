using UnityEngine;

public class FollowObject : MonoBehaviour
{
  public Transform TargetTransform;
  void LateUpdate()
  {
    if (TargetTransform != null)
    {
      // y position adjustment to avoid player being always mid screen
      float adjustedY = TargetTransform.position.y / 5;
      transform.position = new Vector3(TargetTransform.position.x, transform.position.y, transform.position.z);

    }
  }
}
