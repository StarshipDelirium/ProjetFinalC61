using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
  public Animator Animator;
  private GameManager instance;               // Prompt message to appear when interactable object near player

  private void Start()
  {
    instance = GameManager.Instance;
  }
  public void ShowPrompt()
  {
    Animator.SetBool("isPrompt", true);
  }

  public void HidePrompt()
  {
    Animator.SetBool("isPrompt", false);
  }
}
