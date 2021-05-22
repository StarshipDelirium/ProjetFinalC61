using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
  public Dialogue dialogue;

  public void TriggerDialogue()
  {
    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
  }

  /*public void DisplayNextSentence()
  {
    GameManager.Instance.DialogueManager.DisplayNextSentence();
  }*/
}
