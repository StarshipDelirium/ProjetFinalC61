using UnityEngine;

/************************************************************
 * Code reçu de Maël Perreault dans le cadre du cours C63
 ************************************************************/
public class Flash : MonoBehaviour
{
  enum State
  {
    FlashOn,
    FlashOff,
    Done
  }

  public float FlashOnTime = 0.1f;
  public float FlashOffTime = 0.1f;
  public float Duration = 1.0f;

  private State FlashState { get; set; }
  private float FlashStateTimer { get; set; }
  private float DurationTimer { get; set; }

  private Renderer[] _renderers;
  private CanvasGroup[] _canvasGroups;

  public float Alpha
  {
    get
    {
      switch (FlashState)
      {
        case State.FlashOn:
          return 1.0f;
        case State.FlashOff:
          return 0.0f;
      }

      return 1.0f;
    }
  }

  void Start()
  {
    _renderers = gameObject.GetComponentsInChildren<Renderer>();
    _canvasGroups = gameObject.GetComponentsInChildren<CanvasGroup>();

    //StartFlash();
  }

  void Update()
  {
    if (FlashState == State.Done)
      return;

    FlashStateTimer -= Time.deltaTime;
    if (FlashStateTimer <= 0)
      OnFlashStateDone();

    UpdateAlpha();

    DurationTimer -= Time.deltaTime;
    if (DurationTimer <= 0)
      StopFlash();
  }

  private void UpdateAlpha()
  {
    var alpha = Alpha;

    foreach (var renderer in _renderers)
    {
      var color = renderer.material.color;
      renderer.material.color = color.WithAlpha(alpha);
    }

    foreach (var canvasGroup in _canvasGroups)
    {
      canvasGroup.alpha = alpha;
    }
  }

  private void OnFlashStateDone()
  {
    switch (FlashState)
    {
      case State.FlashOn:
        {
          FlashStateTimer = FlashOffTime;
          FlashState = State.FlashOff;
        }
        break;
      case State.FlashOff:
        {
          FlashStateTimer = FlashOnTime;
          FlashState = State.FlashOn;
        }
        break;
    }
  }

  public void StartFlash()
  {
    //Debug.Log("START FLASH");
    enabled = true;
    DurationTimer = Duration;
    FlashStateTimer = FlashOnTime;
    FlashState = State.FlashOn;
    //Debug.Log("END START FLASH");
  }

  public void StopFlash()
  {
    enabled = false;
    DurationTimer = 0.0f;
    FlashState = State.Done;
    UpdateAlpha();
  }
}
