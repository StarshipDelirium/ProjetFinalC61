using UnityEngine;

public class HideOnStart : MonoBehaviour
{
  public Renderer Renderer { get; private set; }

  private void Awake()
  {
    Renderer = GetComponent<Renderer>();
  }

  private void Start()
  {
    Renderer.enabled = false;
  }
}
