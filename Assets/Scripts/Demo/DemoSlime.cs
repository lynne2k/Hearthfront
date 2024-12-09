using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SlimeData
{
    public string name;
    public int x;
    public int y;
    public int z;
}



public class DemoSlime : Mobile
{
    // Start is called before obilethe first frame update


    public override string Save()
    {
        var positionInt = RoundVector3Int(transform.position);
        SlimeData data = new()
        {
            name = "slime",
            x = positionInt.x,
            y = positionInt.y,
            z = positionInt.z
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

    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z; // Keep the Z position the same for 2D
            mousePosition = RoundVector3(mousePosition);
            transform.position = mousePosition;
        }
    }



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
            (int) roundedVector.x,
            (int) roundedVector.y,
            (int) roundedVector.z
        );
    }
}
