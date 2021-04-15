using UnityEngine;

public class AutoScroll : MonoBehaviour
{
  private float scrollSpeed = 0.5f;

  void Update()
  {
    transform.position += Vector3.right * Time.deltaTime * scrollSpeed;

  }

}
