using UnityEngine;
using UnityEngine.SceneManagement;

/**********************************************************
 * With tutorial « How to Fade Between Scenes in Unity ». 
 * https://www.youtube.com/watch?v=Oadq-IrOazg
 **********************************************************/
public class LevelTransition : MonoBehaviour
{
  private Animator animator;
  private int levelToLoad;

  private void Awake()
  {

    animator = gameObject.GetComponent<Animator>();
    Debug.Log("INSIDE AWAKE");
  }
  public void FadeToLevel(int levelIndex)
  {
    Debug.Log("INSIDE FADETOLEVEL");
    levelToLoad = levelIndex;
    animator.SetTrigger("FadeOut");
  }

  public void OnFadeComplete()
  {
    SceneManager.LoadScene(levelToLoad);
  }

}
