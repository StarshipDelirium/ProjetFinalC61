using UnityEngine;

[System.Serializable]

/************************************************************************
* Following tutorial https://www.youtube.com/watch?v=_nRzoTzeyxU&t=254s *
************************************************************************/
public class Dialogue
{
  public string CharacterName;
  [TextArea(1, 5)]                  // Minimum and maximum number of lines each dialog element will have
  public string[] sentences;
}
