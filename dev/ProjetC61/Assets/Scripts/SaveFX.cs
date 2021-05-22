using UnityEngine;

public class SaveFX : MonoBehaviour
{
  public Animator Animator;
  private void Awake()
  {
    Animator = GetComponent<Animator>();
  }
  public void OnAnimationComplete()
  {
    Destroy(gameObject);
  }
}
