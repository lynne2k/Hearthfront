using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MagentaSource : Mobile
{

    public AudioSource bgmSource;

    public override string Save()
    {
        var positionInt = GameUtils.RoundVector3Int(transform.position);
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
        // Trigger End game cutscene!
        bgmSource.Stop();
        //GetComponent<AudioSource>().Play();
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