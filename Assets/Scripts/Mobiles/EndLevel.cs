using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : Mobile
{

    public string sceneName;

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

    }

    private void Update()
    {
        gridPosition = GameUtils.RoundVector3Int(transform.position);
        if (isPossessed && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("End!");
            
            SceneManager.LoadScene(sceneName);
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