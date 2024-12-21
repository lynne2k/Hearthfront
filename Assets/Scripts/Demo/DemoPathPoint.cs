using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PathPointData
{
    public string name;
    public bool gh;
    public int x;
    public int y;
    public int z;
}


public class DemoPathPoint : Mobile
{
    public override string Save()
    {
        var positionInt = RoundVector3Int(transform.position);
        SlimeData data = new()
        {
            name = "slime",
            x = positionInt.x,
            y = positionInt.y,
            z = positionInt.z,
            gh = isPossessed
        };

        return JsonUtility.ToJson(data);
    }

    public override void Load(string loadedData)
    {
        SlimeData data = JsonUtility.FromJson<SlimeData>(loadedData);
        transform.position = new Vector3(
            data.x,
            data.y,
            data.z
        );
        isPossessed = data.gh;

    }

    private void Update()
    {
        gridPosition = GameUtils.RoundVector3Int(transform.position);
    }

    public override void OnTick(int tick)
    {
        return;
    }

    public override void OnPossess()
    {
    }

    public override void OnUnpossess()
    {
    }












    // ------------------------------------------------------
    private Vector3 RoundVector3(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y),
            Mathf.Round(vector.z)
        );
    }

    private Vector3Int RoundVector3Int(Vector3 vector)
    {
        var roundedVector = RoundVector3(vector);
        return new Vector3Int(
            (int)roundedVector.x,
            (int)roundedVector.y,
            (int)roundedVector.z
        );
    }
}