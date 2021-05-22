using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
  public Text Name;                        // display name of character talking, empty string if game info
  public Text DialogText;                           // text to be displayed
  public Animator Animator;
  private Queue<string> sentences;                  // to store relevant dialog, First In First Out collection

  private GameManager instance;
  void Start()
  {
    instance = GameManager.Instance;
    sentences = new Queue<string>();
  }

  internal void StartDialogue(Dialogue dialogue)      // when triggered by a DialogueTrigger
  {
    Debug.Log("Starting conversation with " + dialogue.CharacterName);
    Animator.SetBool("isActive", true);             // parameter added in Unity Animator to transition between hide/show dialogue box on screen
    Name.text = dialogue.CharacterName;

    sentences.Clear();                                          // clear any previous dialogue

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
      EndDialogue();
      return;
    }

    string sentence = sentences.Dequeue();
    StopAllCoroutines();                                    // stop coroutine of previous phrase before generating next one
    StartCoroutine(DisplaySentence(sentence));
  }

  IEnumerator DisplaySentence(string sentence)
  {
    DialogText.text = "";                                     // erase previous phrase from dialogue box

    yield return new WaitForSeconds(0.1f);

    foreach (char letter in sentence.ToCharArray())
    {
      DialogText.text += letter;
      yield return 1;                                         // return each letter in iteration for a typing effect
    }
  }

  public void EndDialogue()
  {
    Animator.SetBool("isActive", false);                      // transitions to hidden dialogue box
    Debug.Log("End of conversation");
  }
}
