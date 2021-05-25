using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/****************************************************************
 *  « How to make a Dialogue System in Unity ». 
 *  https://www.youtube.com/watch?v=_nRzoTzeyxU&t=254s
  **************************************************************/
public class DialogueManager : MonoBehaviour
{
  public Text Name;                        // display name of character talking, empty string if game info
  public Text DialogText;                           // text to be displayed
  public Text PrologueText;
  public IntroDialogue intro;
  public Animator Animator;
  private Queue<string> sentences;                  // to store relevant dialog, First In First Out collection
  private Text TextArea;
  private float typingSpeed;

  private GameManager instance;
  void Start()
  {
    instance = GameManager.Instance;
    sentences = new Queue<string>();
    intro = GetComponent<IntroDialogue>();
  }

  private void OnEnable()
  {
    if ("Prologue".Equals(SceneManager.GetActiveScene().name))                        // to use DialogueManager with different Animators
    {
      Debug.Log("THIS IS PROLOGUE");
      TextArea = PrologueText;
      typingSpeed = 0.5f;
    }
    else
    {
      Debug.Log("THIS IS NOT PROLOGUE");
      TextArea = DialogText;
      typingSpeed = 0.1f;
    }
  }

  internal void StartDialogue(Dialogue dialogue)      // when triggered by a DialogueTrigger
  {

    if (!"Prologue".Equals(SceneManager.GetActiveScene().name))
    {
      Animator.SetBool("isActive", true);             // parameter added in Unity Animator to transition between hide/show dialogue box on screen
      Name.text = dialogue.CharacterName;
    }

    if (sentences != null)
    {
      sentences.Clear();                              // clear any previous dialogue
    }

    foreach (string sentence in dialogue.sentences)
    {
      sentences.Enqueue(sentence);                              // Add each sentence from dialogue trigger to queue to be read
    }

    DisplayNextSentence();
  }

  public void DisplayNextSentence()
  {
    if (sentences.Count == 0)                                // as sentences are dequeued to be read
    {
      if ("Prologue".Equals(SceneManager.GetActiveScene().name))
      {
        StopAllCoroutines();                                    // stop coroutine of previous phrase before generating next one
        StartCoroutine(DisplaySentence(""));
        EndPrologue();
        return;
      }
      else
      {
        EndDialogue();
        return;
      }

    }

    string sentence = sentences.Dequeue();
    StopAllCoroutines();                                    // stop coroutine of previous phrase before generating next one
    StartCoroutine(DisplaySentence(sentence));
  }

  IEnumerator DisplaySentence(string sentence)
  {
    TextArea.text = "";                                     // erase previous phrase from dialogue box

    yield return new WaitForSeconds(typingSpeed);

    foreach (char letter in sentence.ToCharArray())
    {
      TextArea.text += letter;
      yield return 1;                                         // return each letter in iteration for a typing effect
    }
  }

  public void EndDialogue()
  {
    Animator.SetBool("isActive", false);                      // transitions to hidden dialogue box
    Debug.Log("End of conversation");
    Debug.Log("Current Scene Name: " + SceneManager.GetActiveScene().name);

    if ("Prologue".Equals(SceneManager.GetActiveScene().name))
    {
      EndPrologue();
    }
  }

  public void EndPrologue()
  {
    FindObjectOfType<IntroDialogue>().OnPrologueEnd();
  }
}
