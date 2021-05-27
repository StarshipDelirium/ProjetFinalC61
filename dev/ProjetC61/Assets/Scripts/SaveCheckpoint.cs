using UnityEngine;

public class SaveCheckpoint : MonoBehaviour, IInteractable, ISaveable
{
  public int ID;
  public Dialogue dialogue;
  public Transform VFX;
  private bool isActivated = false;
  private bool interacted;
  public delegate void SaveCheckpointEvent(SaveCheckpoint checkpoint);

  public SaveCheckpointEvent OnChanged;

  private int _value;

  private void Awake()
  {
    _value = ID;
  }

  public int Value                          // keep track of save checkpoints where user has saved game
  {
    get { return _value; }
    set
    {
      var previous = _value;

      if (_value != previous)              // new checkpoint reached
      {
        OnChanged?.Invoke(this);

      }
    }
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
  public void Save()
  {
    GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Save);
    GameManager.Instance.SaveLoadManager.SaveGameData(this.ID);
    GameManager.Instance.PrefabManager.Spawn(PrefabManager.Vfx.SaveFX, VFX.position, gameObject.transform.rotation);
    FindObjectOfType<DialogueManager>().DisplayNextSentence();
  }
}
