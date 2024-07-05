using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public static class ExtentionMethods 
{
    public static Vector2 GetWorldPos(this Vector2 v2)
    {
        return Camera.main.ScreenToWorldPoint(v2);
    }

    public static Vector2 GetWorldPos(this Vector3 v3)
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(v3);
    }

    public static Vector2 GetScreenPos(this Vector2 v2)
    {
        return Camera.main.WorldToScreenPoint(v2);
    }

    public static Vector3 With(this Vector3 v3, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? v3.x, y ?? v3.y, z ?? v3.z);
    }

    public static Directions Quaternion2Direction(this Quaternion quaternion)
    {
        if (quaternion == Quaternion.Euler(0, 0, 0))
        {
            return Directions.Right;
        }
        else if (quaternion == Quaternion.Euler(0, 0, 90))
        {
            return Directions.Top;
        }
        else if (quaternion == Quaternion.Euler(0, 0, 180))
        {
            return Directions.Left;
        }
        else if (quaternion == Quaternion.Euler(0, 0, 270))
        {
            return Directions.Bottom;
        }

        return Directions.Right;
    }
    
    public static Quaternion Direction2Quaternion(this Directions direction)
    {
        if (direction == Directions.Right)
        {
            return Quaternion.Euler(0, 0, 0);  // right
        }
        else if (direction == Directions.Top)
        {
            return Quaternion.Euler(0, 0, 90);  // top
        }
        else if (direction == Directions.Left)
        {
            return Quaternion.Euler(0, 0, 180);  // top
        }
        else if (direction == Directions.Bottom)
        {
            return Quaternion.Euler(0, 0, 270); // down
        }

        return Quaternion.Euler(0, 0, 0);
    }
}