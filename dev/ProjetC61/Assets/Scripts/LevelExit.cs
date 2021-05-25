using UnityEngine;

/************************************************************
 * Code reçu de Maël Perreault dans le cadre du cours C63
 ************************************************************/
public class LevelExit : MonoBehaviour
{
  public LevelManager.Level Level;
  public string LevelEntranceId;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    var player = collision.GetComponent<Player>();
    if (player)
    {
      GameManager.Instance.LevelManager.GoToLevel(Level, LevelEntranceId);
    }
  }
}
