using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Static instance to ensure global access
    public static GameManager Instance { get; private set; }


    /* Global Variables -- 登记各种全局变量！ */



    public const int framePerTick = 30;


    public int currentTick = 0;
    public int currentTickTimeDelta = 0;

    
    public int varFoo = 0;
    public string varBar = "Player";
    public Vector3Int varTestCoord = new Vector3Int(1, 3, 3);

    /* 全图Savable寄存 */

    public List<string> snapshots = new List<string>();
    public Mobile[] allMobiles;


    /* 确保唯一性 */

    private void Awake()
    {
        // Check if another instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes

        allMobiles = FindObjectsOfType<Mobile>();
    }



    /* TEMPORARY: 输入检测不应该放在GameManager里面。仅限debug，之后会移除 */


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadSnapshot(currentTick - 1);
            currentTick--;
            currentTickTimeDelta = 0;

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //TakeSnapshot();

        }
    }

    private void FixedUpdate()
    {
        /* time++； 如果到时间了就tick一次：更新所有东西  */

        currentTickTimeDelta++;
        if ((currentTickTimeDelta+1) % framePerTick == 0)
        {
            currentTick++;
            Debug.Log(currentTick);
            TakeSnapshot();

            foreach (Mobile mob in allMobiles)
            {
                mob.onTick(currentTick);
            }
        }
    }



    private void TakeSnapshot()
    {
        // Create a dictionary to hold all entities' JSON data
        var snapshotData = new Dictionary<string, string>();

        // Iterate over all saveable entities
        foreach (var mob in allMobiles)
        {
            // Serialize each entity's state
            string json = mob.Save();
            snapshotData[mob.name] = json; // Use entity name as the key
        }

        // Convert the snapshot to a JSON string
        string snapshotJson = JsonUtility.ToJson(new SerializableDictionary(snapshotData));

        // Add the snapshot JSON to the list
        snapshots.Add(snapshotJson);

        Debug.Log($"Snapshot taken: {snapshotJson}");
    }

    private void LoadSnapshot(int tickToLoad)
    {
        SerializableDictionary serialDict = JsonUtility.FromJson<SerializableDictionary>(snapshots[tickToLoad]);
        Dictionary<string, string> dict = serialDict.ToDictionary();

        foreach (Mobile mob in allMobiles)
        {
            mob.Load(dict[mob.name]);
        }

        //Debug.Log("wow awesome");

    }
}
