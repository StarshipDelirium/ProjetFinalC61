using UnityEngine;

public class LevelMusic : MonoBehaviour
{
  public SoundManager.Music Music = SoundManager.Music.Music;

  private void Awake()
  {
    GameManager.Instance.SoundManager.Play(Music);
  }
}
