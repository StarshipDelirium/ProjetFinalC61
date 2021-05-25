using UnityEngine;

/*************************************************************************
 *  Adapté du code reçu Maël Perreault et créé dans le cadre du cours C63
 *************************************************************************/
public class SoundManager : MonoBehaviour
{

  public enum Music
  {
    FestivalOfSpirits,
    Music,
    UnholyIllusions,

    Count
  }

  public enum Sfx
  {
    Attack,
    Hurt,
    Jump,
    Kil,
    Rise,

    Count
  }

  public AudioClip[] MusicAudioClips;
  public AudioClip[] SfxAudioClips;
  public AudioSource MusicAudioSource { get; private set; }
  void Awake()
  {
    MusicAudioClips = Resources.LoadAll<AudioClip>("audio/music");
    Debug.Assert((int)Music.Count == MusicAudioClips.Length, "SoundManager : Music enum length (" + (int)Music.Count + ") does not match Resources folder (" + MusicAudioClips.Length + ")");

    SfxAudioClips = Resources.LoadAll<AudioClip>("audio/sfx");
    Debug.Assert((int)Sfx.Count == SfxAudioClips.Length, "SoundManager : Sfx enum length " + (int)Sfx.Count + ") does not match Resources folder (" + SfxAudioClips.Length + ")");

    //MusicAudioSource = gameObject.AddComponent<AudioSource>();
    MusicAudioSource = gameObject.AddComponent<AudioSource>();
    MusicAudioSource.loop = true;
    MusicAudioSource.volume = 0.06f;
  }

  public void Play(Music music)
  {
    MusicAudioSource.clip = MusicAudioClips[(int)music];
    MusicAudioSource.Play();
  }

  public void Play(Sfx sfx)
  {
    MusicAudioSource.clip = MusicAudioClips[(int)sfx];
    MusicAudioSource.Play();
  }

  public void Stop()
  {
    MusicAudioSource.Stop();
  }

}
