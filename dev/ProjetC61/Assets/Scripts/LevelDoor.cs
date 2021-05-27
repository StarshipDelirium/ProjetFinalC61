using UnityEngine;

public class LevelDoor : MonoBehaviour, IInteractable
{
  public LevelExit exit;
  public Dialogue dialogue;
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
      SpawnBoss();
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

  public void SpawnBoss()
  {
    Vector3 spawnPoint = new Vector3(63.75f, -2.47f, 0f);                                                                      // position near fight area
    GameManager.Instance.PrefabManager.Spawn(PrefabManager.Enemy.Possessed, spawnPoint, Quaternion.identity);
  }
}
