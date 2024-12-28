using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SlimeData
{
    public string name;
    public bool gh;
    public int x;
    public int y;
    public int z;
    public int stamina;
    public int fx;
    public int fy;
}



public class DemoSlime : Mobile
{
    // Start is called before obilethe first frame update



    private Vector3Int targetPositionBuffer;
    private Renderer objectRenderer;
    private bool isMoving;
    public int initialStamina = 3;
    public int stamina = 3;
    private int facingX = 1;
    private int facingY = 1;




    public LayerMask solidLayer;


    void Start()
    {
        stamina = initialStamina;

        objectRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (objectRenderer != null)
        {
            Debug.Log("did I find?" + objectRenderer.gameObject.name);
        }
    }



    public override string Save()
    {
        SlimeData data = new()
        {
            name = "slime",
            x = gridPosition.x,
            y = gridPosition.y,
            z = gridPosition.z,
            gh = isPossessed,
            stamina = stamina,
            fx = facingX,
            fy = facingY
        };

        return JsonUtility.ToJson(data);
    }

    public override void Load(string loadedData)
    {
        SlimeData data = JsonUtility.FromJson<SlimeData>(loadedData);
        gridPosition = new Vector3Int(
            data.x, data.y, data.z
        );
        transform.position = new Vector3(
            data.x,
            data.y,
            data.z
        );
        //isPossessed = data.gh;
        stamina = data.stamina;
        facingX = data.fx;
        facingY = data.fy;

    }


    public override void OnTick(int tick)
    {
        if (isMoving)
        {
            transform.position = targetPositionBuffer;
            gridPosition = targetPositionBuffer;  // RoundVector3Int(transform.position);
            isMoving = false;
            stamina -= 1;
        }
        
    }
    public override void OnPossess()
    {
        isPossessed = true;
    }

    public override void OnUnpossess()
    {
        isPossessed = false;
    }

    // Update is called once per frame
    void Update()
    {

        //if (isPossessed && Input.GetMouseButtonDown(0) && !isMoving && stamina > 0) // Left mouse button
        //{
        //    // Get the mouse position in world space
        //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    mousePosition.z = transform.position.z; // Keep the Z position the same for 2D
        //    mousePosition = RoundVector3(mousePosition);
        //    targetPositionBuffer = mousePosition;
        //    isMoving = true;
        //}
        if (isPossessed && !isMoving && stamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.D) && TestMove(new Vector3Int(1, 0, 0)))
            {
                isMoving = true;
                targetPositionBuffer = gridPosition + new Vector3Int(1, 0, 0);
                GameManager.Instance.NotifyMobileUpdate();
                Debug.Log(targetPositionBuffer.ToString());
            }
            else if (Input.GetKeyDown(KeyCode.A) && TestMove(new Vector3Int(-1, 0, 0)))
            {
                isMoving = true;
                targetPositionBuffer = gridPosition + new Vector3Int(-1, 0, 0);
                GameManager.Instance.NotifyMobileUpdate();
                Debug.Log(targetPositionBuffer.ToString());
            }
            else if (Input.GetKeyDown(KeyCode.W) && TestMove(new Vector3Int(0, 1, 0)))
            {
                isMoving = true;
                targetPositionBuffer = gridPosition + new Vector3Int(0, 1, 0);
                GameManager.Instance.NotifyMobileUpdate();
                Debug.Log(targetPositionBuffer.ToString());
            }
            else if (Input.GetKeyDown(KeyCode.S) && TestMove(new Vector3Int(0, -1, 0)))
            {
                isMoving = true;
                targetPositionBuffer = gridPosition + new Vector3Int(0, -1, 0);
                GameManager.Instance.NotifyMobileUpdate();
                Debug.Log(targetPositionBuffer.ToString());
            }
            

        }
    }


    private bool TestMove(Vector3Int vec)
    {
        if (Physics2D.OverlapArea(gridPosition + vec - new Vector3(0.2f, 0.2f, 0), gridPosition + vec + new Vector3(0.2f, 0.2f, 0), solidLayer))
            return false;
        return true;
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
