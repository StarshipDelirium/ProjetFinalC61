using UnityEngine;

static public class Vector3Extensions
{
  public static Vector2 XY(this Vector3 v)
  {
    return new Vector2(v.x, v.y);
  }

  public static Vector3 XY0(this Vector3 v)
  {
    return new Vector3(v.x, v.y, 0);
  }

  public static Vector3 XY1(this Vector3 v)
  {
    return new Vector3(v.x, v.y, 1);
  }

  public static Vector3 XYZ(this Vector2 v, float z)
  {
    return new Vector3(v.x, v.y, z);
  }

  public static Vector3 WithX(this Vector3 v, float x)
  {
    return new Vector3(x, v.y, v.z);
  }

  public static Vector3 WithY(this Vector3 v, float y)
  {
    return new Vector3(v.x, y, v.z);
  }

  public static Vector3 WithZ(this Vector3 v, float z)
  {
    return new Vector3(v.x, v.y, z);
  }

  public static Quaternion ToQuaternion(this Vector3 vector)
  {
    return Quaternion.Euler(vector);
  }
}