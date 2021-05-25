using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMusic : MonoBehaviour
{
  public SoundManager.Music Music;

  private void Awake()
  {
    var currentLevel = SceneManager.GetActiveScene().buildIndex;

    switch (currentLevel)
    {
      case 0:
        Music = SoundManager.Music.FestivalOfSpirits;
        break;
      case 1:
      case 2:
        Music = SoundManager.Music.UnholyIllusions;
        break;
      default:
        break;
    }


    GameManager.Instance.SoundManager.Play(Music);
  }
}
