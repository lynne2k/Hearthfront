using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    // Utility function to round coordinates to integer values
    public static Vector3 RoundVector3(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y),
            Mathf.Round(vector.z)
        );
    }

    public static Vector3Int RoundVector3Int(Vector3 vector)
    {
        var roundedVector = RoundVector3(vector);
        return new Vector3Int(
            (int) roundedVector.x,
            (int) roundedVector.y,
            (int) roundedVector.z
        );
    }

    public static Vector2 CubicBezierVector2(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        // Perform cubic interpolation
        Vector2 p = uuu * p0; // (1 - t)^3 * P0
        p += 3 * uu * t * p1; // 3 * (1 - t)^2 * t * P1
        p += 3 * u * tt * p2; // 3 * (1 - t) * t^2 * P2
        p += ttt * p3;         // t^3 * P3

        return p;
    }
}