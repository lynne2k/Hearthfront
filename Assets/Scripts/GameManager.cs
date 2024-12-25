using UnityEngine;
using System.Collections.Generic;
using System.IO;


public enum TimeMode
{
    AUTO, MANUAL
}

public class GameManager : MonoBehaviour
{
    // Static instance to ensure global access
    public static GameManager Instance { get; private set; }


    /* Global Variables -- 登记各种全局变量！ */

    private const int framePerTick = 30;

    protected int currentTick = 0;
    protected int currentTickTimeDelta = 0;
    protected float timePassedSinceLastTick = 0;

    /* 全图Savable寄存 */
    private const int chunk_size = 5;
    private string[] snapshots = new string[chunk_size];
    public bool isLock = false;

    public TimeMode timeflowOption = TimeMode.AUTO;

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
         
        // 初始时snapshot

        

    }

    private void Start()
    {
        //Ghost.Instance.Initiate(); # private
        TakeSnapshot(0);
    }



    /* TEMPORARY: 输入检测不应该放在GameManager里面。仅限debug，之后会移除 */

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isLock)
        {
            isLock = true;
            if (currentTick >= 1)
            {
                bool Loaded = LoadSnapshot(currentTick - 1);
                if (Loaded)
                {
                    currentTick--;
                    //currentTickTimeDelta = 0;
                    timePassedSinceLastTick = 0f;
                }
            }
            isLock = false;
            /*Debug.Log($"currentTick: {currentTick}");*/
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            timeflowOption = timeflowOption == TimeMode.AUTO ? TimeMode.MANUAL : TimeMode.AUTO;
        }


        /* Ticking */
        bool isTickingThisFrame = false;

        timePassedSinceLastTick += Time.deltaTime;
        if (timeflowOption == TimeMode.AUTO && timePassedSinceLastTick >= 1f)    // Using absolute time, instead of frames...
        {
            isTickingThisFrame = true;
            timePassedSinceLastTick = 0f;
        }
        else if (timeflowOption == TimeMode.MANUAL && Input.GetKeyDown(KeyCode.P))
        {
            isTickingThisFrame = true;
            timePassedSinceLastTick = 0f;
        }


        // Actual Ticking
        if (isTickingThisFrame && currentTick < 600)
        {
            isLock = true; // 设置读写锁

            currentTick++;

            TakeSnapshot(currentTick);
            foreach (Mobile mob in allMobiles)
            {
                mob.OnTick(currentTick);
            }
            Ghost.Instance.OnTick();
            isLock = false;
        }

    }


    private void TakeSnapshot(int tick)
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
        int index = tick % chunk_size;
        /*Debug.Log($"index: {index}");*/
        /*Debug.Log($"snapshots: {snapshots.Length}");*/
        snapshots[index] = snapshotJson;

        if ((currentTick + 1) % chunk_size == 0)
        {
            WriteSnapshotsToDisk();
        }
        /*Debug.Log($"Snapshot taken: {snapshotJson}");*/
    }

    private void WriteSnapshotsToDisk()
    {
        var bigDict = new Dictionary<int, string>();
        
        // 保存的Chunk两端的tick序数
        int startTick = currentTick + 1 - chunk_size;
        int endTick = currentTick;
        // 填充chunk字典
        for (int i = startTick; i <= endTick; i++)
        {
            bigDict[i] = snapshots[i % chunk_size];
        }

        // 转为字符串格式并存储，此时无需清理snapshots，后续直接覆盖
        string bigJson = JsonUtility.ToJson(new IntKeyedSerializableDictionary(bigDict));
        string filePath = GetSnapshotFilePath(startTick, endTick);
        File.WriteAllText(filePath, bigJson);
        Debug.Log($"Wrote snapshots {startTick} to {endTick} to disk at {filePath}");
    }

    private bool LoadSnapshot(int tickToLoad)
    {
        /* !!!!!!!!!!!!!!! tickToLoad!!! */
        if ((currentTick + 1) % chunk_size != 0) // 内存中
        {
            int index = currentTick % chunk_size;
            string snapshotJson = snapshots[index];
            ApplySnapshot(snapshotJson);
            return true;
        }
        else
        {
            int index = currentTick % chunk_size;
            int startTick = currentTick - index;
            int endTick = currentTick;
            string filePath = GetSnapshotFilePath(startTick, endTick);
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"No snapshot file found for tick !");
                return false;
            }

            string bigJson = File.ReadAllText(filePath);
            IntKeyedSerializableDictionary fileDict = JsonUtility.FromJson<IntKeyedSerializableDictionary>(bigJson);
            Dictionary<int, string> bigDict = fileDict.ToDictionary();
            
            bigDict.TryGetValue(currentTick, out string snapshotJson);
            
            ApplySnapshot(snapshotJson);

            foreach (var kvp in bigDict)
            {
                int tick = kvp.Key;
                string snapshot = kvp.Value;
                int snapIndex = tick % chunk_size;
                snapshots[snapIndex] = snapshot;
            }

            return true;
        }
    }


    private string GetSnapshotFilePath(int startTick, int endTick)
    {
        string dir = Path.Combine(Application.persistentDataPath, "Snapshots");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return Path.Combine(dir, $"snapshot_{startTick}_{endTick}.json");
    }


    private void ApplySnapshot(string snapshotJson)
    {
        SerializableDictionary serialDict = JsonUtility.FromJson<SerializableDictionary>(snapshotJson);
        Dictionary<string, string> dict = serialDict.ToDictionary();

        foreach (Mobile mob in allMobiles)
        {
            if (dict.ContainsKey(mob.name))
            {
                mob.Load(dict[mob.name]);
            }
        }
        /*Debug.Log("Snapshot applied.");*/
    }


    /* ---------------------------- Util methods ---------------------------- */
    public int GetCurrentTick()
    {
        return currentTick;
    }

    public Mobile FindMobileByCoordinate(Vector3Int gridPosition)
    {
        foreach (Mobile mob in allMobiles)
        {
            if (mob.gridPosition.x == gridPosition.x && mob.gridPosition.y == gridPosition.y)
            {
                return mob;
            }
        }
        return null;
    }
}
