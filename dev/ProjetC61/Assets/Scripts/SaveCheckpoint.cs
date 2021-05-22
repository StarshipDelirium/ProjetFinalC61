using UnityEngine;

public class SaveCheckpoint : MonoBehaviour, IInterractable, ISaveable
{
  public int ID;
  public Dialogue dialogue;
  public Transform VFX;
  private bool isActivated = false;
  private bool interacted;
  private string prompt = "";
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

  void OnGUI()
  {
    if (isActivated)
    {
      Debug.Log("ON GUI");
      GUI.Box(new Rect(100, Screen.height - 150, Screen.width - 150, 100), (prompt));
    }
  }

  public void Prompt()
  {
    prompt = "Press 'E' to interact";
    isActivated = true;

  }

  public void CancelInteraction()
  {
    isActivated = false;
    interacted = false;
    FindObjectOfType<DialogueManager>().EndDialogue();
  }

  public void OnTriggerExit2D(Collider2D collider)
  {
    isActivated = false;
    interacted = false;

  }

  public void Save()
  {

    GameManager.Instance.SaveLoadManager.SaveGameData(this.ID);
    GameManager.Instance.PrefabManager.Spawn(PrefabManager.Vfx.SaveFX, VFX.position, gameObject.transform.rotation);
    FindObjectOfType<DialogueManager>().DisplayNextSentence();
  }
}
