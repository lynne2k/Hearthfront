using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PathPointData
{
    public string name;
    public bool gh;
}


public class DemoPathPoint : Mobile
{
    public override string Save()
    {
        var positionInt = RoundVector3Int(transform.position);
        PathPointData data = new()
        {
            name = gameObject.name,
            gh = isPossessed
        };

        return JsonUtility.ToJson(data);
    }

    public override void Load(string loadedData)
    {
        PathPointData data = JsonUtility.FromJson<PathPointData>(loadedData);
        //isPossessed = data.gh;

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
        isPossessed = true;
    }

    public override void OnUnpossess()
    {
        isPossessed = false;
    }


    private void Start()
    {
        gridPosition = GameUtils.RoundVector3Int(transform.position);
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