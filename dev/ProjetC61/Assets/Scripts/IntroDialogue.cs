using UnityEngine;

public class IntroDialogue : MonoBehaviour
{
  public Dialogue dialogue;
  public LevelTransition transition;

  private void Awake()
  {
  }

  void Start()
  {
    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyUp(KeyCode.E))
    {
      FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }
  }

  public void OnPrologueEnd()
  {
    //transition.FadeToLevel(2);
  }
}
