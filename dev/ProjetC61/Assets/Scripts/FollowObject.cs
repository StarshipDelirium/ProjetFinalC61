using UnityEngine;

public class FollowObject : MonoBehaviour
{
  public Transform TargetTransform;

  // Update is called once per frame
  void LateUpdate()
  {
    if (TargetTransform != null)
    {

      transform.position = TargetTransform.position.WithZ(transform.position.z);
    }

  }
}
