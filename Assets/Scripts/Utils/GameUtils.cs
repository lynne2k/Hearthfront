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
}