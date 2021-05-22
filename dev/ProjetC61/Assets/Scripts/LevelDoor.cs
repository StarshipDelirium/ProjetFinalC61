using UnityEngine;

public class LevelDoor : MonoBehaviour, IInterractable
{
  public LevelExit exit;
  public Dialogue dialogue;
  private bool interacted;
  private bool isActivated = false;
  private string prompt = "";

  private void Start()
  {
    //exit = gameObject.GetComponent<LevelExit>();
    //exit.GetComponent<BoxCollider2D>().enabled = false;                       // disabled until condition is met (i.e. obtain key)
  }

  public void Interact()
  {
    Debug.Log("LEVEL DOOR INTERACT");


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
  void OnGUI()
  {
    if (isActivated)
    {
      Debug.Log("ON GUI");
      GUI.Box(new Rect(140, Screen.height - 50, Screen.width - 300, 120), (prompt));
    }
  }

  public void Prompt()
  {
    prompt = "Press 'E' to interact";
    isActivated = true;

  }

  private void OnTriggerExit2D(Collider2D other)
  {
    isActivated = false;
  }

  public void CancelInteraction()
  {
    isActivated = false;
    interacted = false;
    FindObjectOfType<DialogueManager>().EndDialogue();

  }
}
