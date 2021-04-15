using UnityEngine;
using UnityEngine.SceneManagement;

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
