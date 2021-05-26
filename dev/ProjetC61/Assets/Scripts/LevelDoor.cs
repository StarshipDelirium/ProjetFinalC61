using UnityEngine;

public class LevelDoor : MonoBehaviour, IInteractable
{
  public LevelExit exit;
  public Dialogue dialogue;
  public bool hasReachedCrypt;
  public LevelBoss LevelBoss;
  private bool interacted;
  private bool isActivated = false;

  private void Awake()
  {
    LevelBoss = GetComponent<LevelBoss>();
  }

  public void Interact()
  {
    if (!interacted)
    {
      FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
      interacted = true;


    }
    else
    {
      FindObjectOfType<DialogueManager>().DisplayNextSentence();
      interacted = false;
    }

  }
  public void Prompt()
  {
    if (!isActivated)
    {
      FindObjectOfType<InteractPrompt>().ShowPrompt();
      isActivated = true;
    }
  }

  public void CancelInteraction()
  {
    isActivated = false;
    interacted = false;
    FindObjectOfType<DialogueManager>().EndDialogue();
    FindObjectOfType<InteractPrompt>().HidePrompt();
  }
}
