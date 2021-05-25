using UnityEngine;

/************************************************************
 * Code reçu de Maël Perreault dans le cadre du cours C63
 ************************************************************/
public class Fade : MonoBehaviour
{
  enum State
  {
    Start,
    FadeIn,
    Wait,
    FadeOut,
    Done,
  }

  public float FadeInTime = 0;
  public float FadeWaitTime = 0;
  public float FadeOutTime = 1;
  public bool DestroyOnFadeOut = false;

  private State FadeState { get; set; }
  private float FadeTimer { get; set; }

  private float _lastAlpha;
  private Renderer[] _renderers;
  private CanvasGroup[] _canvasGroups;

  public float Alpha
  {
    get
    {
      switch (FadeState)
      {
        case State.Start:
          return 0.0f;
        case State.FadeIn:
          return 1.0f - FadeTimer / FadeInTime;
        case State.Wait:
          return 1.0f;
        case State.FadeOut:
          return FadeTimer / FadeOutTime;
        case State.Done:
          return _lastAlpha;
      }

      return 1.0f;
    }
  }

  void Start()
  {
    _renderers = gameObject.GetComponentsInChildren<Renderer>();
    _canvasGroups = gameObject.GetComponentsInChildren<CanvasGroup>();

    StartFade();
  }

  void Update()
  {
    if (FadeState == State.Done)
      return;

    FadeTimer -= Time.deltaTime;
    if (FadeTimer <= 0)
      OnFadeDone();
    else
      UpdateAlpha();
  }

  private void UpdateAlpha()
  {
    var alpha = Alpha;
    _lastAlpha = alpha;

    if (_renderers != null)
    {
      foreach (var renderer in _renderers)
      {
        var color = renderer.material.color;
        renderer.material.color = color.WithAlpha(alpha);
      }
    }

    if (_canvasGroups != null)
    {
      foreach (var canvasGroup in _canvasGroups)
      {
        canvasGroup.alpha = alpha;
      }
    }
  }

  private void OnFadeDone()
  {
    if (FadeState == State.Start)
    {
      FadeTimer = FadeInTime;
      FadeState = State.FadeIn;
    }

    if (FadeState == State.FadeIn
        && FadeTimer <= 0)
    {
      FadeTimer = FadeWaitTime;
      FadeState = State.Wait;
    }

    if (FadeState == State.Wait
        && FadeTimer <= 0)
    {
      FadeTimer = FadeOutTime;
      FadeState = State.FadeOut;
    }

    if (FadeState == State.FadeOut
        && FadeTimer <= 0)
    {
      if (DestroyOnFadeOut)
        Destroy(gameObject);

      StopFade();
    }

    UpdateAlpha();
  }
  public void StartFade()
  {
    enabled = true;
    _lastAlpha = 1.0f;
    FadeState = State.Start;

    if (FadeInTime == 0
        && FadeWaitTime == 0
        && FadeOutTime == 0)
    {
      _lastAlpha = 0.0f;
    }

    OnFadeDone();
  }

  public void StopFade()
  {
    enabled = false;
    FadeTimer = 0.0f;
    FadeState = State.Done;
  }
}
