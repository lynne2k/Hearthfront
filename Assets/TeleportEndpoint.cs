using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class TeleportEndPoint : Mobile
{

    public TeleportEndPoint OtherEndpoint;



    public override string Save()
    {
        //var positionInt = GameUtils.RoundVector3Int(transform.position);
        PathPointData data = new()
        {
            name = gameObject.name,
            gh = isPossessed
        };

        return JsonUtility.ToJson(data);
    }

    public override void Load(string loadedData)
    {
        //PathPointData data = JsonUtility.FromJson<PathPointData>(loadedData);
        //isPossessed = data.gh;

    }

    private void Update()
    {
        gridPosition = GameUtils.RoundVector3Int(transform.position);
        if (isPossessed && Input.GetKeyDown(KeyCode.E))
        {
            bool success = Ghost.Instance.CallSwap(OtherEndpoint);
            //Debug.Log(gameObject.name + " trying to teleport to " + OtherEndpoint.name + "with succ:" + success.ToString());
        }
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
}