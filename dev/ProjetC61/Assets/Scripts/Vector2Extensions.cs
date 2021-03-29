using UnityEngine;

static public class Vector2Extensions
{
  public static Vector3 ToVector3(this Vector2 v)
  {
    return new Vector3(v.x, v.y, 0);
  }

  public static Vector3 XY0(this Vector2 v)
  {
    return new Vector3(v.x, v.y, 0);
  }

  public static Vector3 XY1(this Vector2 v)
  {
    return new Vector3(v.x, v.y, 1);
  }

  public static Vector3 XYZ(this Vector2 v, float z)
  {
    return new Vector3(v.x, v.y, z);
  }

  public static Vector2 WithX(this Vector2 v, float x)
  {
    return new Vector2(x, v.y);
  }

  public static Vector2 WithY(this Vector2 v, float y)
  {
    return new Vector2(v.x, y);
  }

  public static Quaternion ToQuaternion(this Vector2 vector)
  {
    return Quaternion.Euler(vector);
  }

  public static Vector2 XY(this Vector3 v)
  {
    return new Vector2(v.x, v.y);
  }

  public static float Lerp(this Vector2 v, float t)
  {
    return Mathf.Lerp(v.x, v.y, t);
  }
}