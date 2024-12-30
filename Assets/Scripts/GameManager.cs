using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public enum TimeMode
{
    AUTO, MANUAL, PAUSED
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    /* Global Variables -- 登记各种全局变量！ */

    private const int framePerTick = 30;

    protected int currentTick = 0;
    protected int currentTickTimeDelta = 0;
    protected float timePassedSinceLastTick = 0;

    /* 全图Savable寄存 */
    private const int chunk_size = 10;
    private Dictionary<int, string> snapshots = new();
    public bool isLock = false;

    public TimeMode timeflowOption = TimeMode.AUTO;

    public Mobile[] allMobiles;

    private const int snapshotInterval = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        SnapshotManager.snapshotInterval = snapshotInterval; // 同步interval到SnapshotManager

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

            GetComponentInChildren<SuperAudioManager>().PlayRewind();
            /*Debug.Log($"currentTick: {currentTick}");*/
        }
        //else if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    timeflowOption = timeflowOption == TimeMode.AUTO ? TimeMode.MANUAL : TimeMode.AUTO;
        //}


        /* Ticking */
        bool isTickingThisFrame = false;

        timePassedSinceLastTick += Time.deltaTime;
        if (timeflowOption == TimeMode.AUTO && timePassedSinceLastTick >= 1f)    // Using absolute time, instead of frames...
        {
            isTickingThisFrame = true;
            timePassedSinceLastTick = 0f;
        }
        else if (timeflowOption == TimeMode.MANUAL && Input.GetKeyDown(KeyCode.Space))
        {
            isTickingThisFrame = true;
            timePassedSinceLastTick = 0f;
        }
        else if (timeflowOption == TimeMode.MANUAL && canGoToNextTick)
        {
            isTickingThisFrame = true;
            canGoToNextTick = false;
            timePassedSinceLastTick = 0f;
        }

        Ghost.Instance.OnTick();
        // Actual Ticking
        if (isTickingThisFrame && currentTick < 600)
        {
            TickOnce();
        }

    }


    private int TickOnce()
    {
        isLock = true; // 设置读写锁

        currentTick++;

        foreach (Mobile mob in allMobiles)
        {
            mob.OnTick(currentTick);
        }

        isLock = false;

        TakeSnapshot(currentTick);

        return currentTick;
    }



    // TakeSnapshot
    //  - WriteSnapshotsToDiskAndFlush
    // LoadSnapshot
    // - ApplySnapshot


    private void TakeSnapshot(int tick)
    {
        var snapshotData = new Dictionary<string, string>(); // all data in this frame
        foreach (var mob in allMobiles)
        {
            string json = mob.Save();
            snapshotData[mob.name] = json;
        }

        string snapshotJson = JsonUtility.ToJson(new SerializableDictionary(snapshotData)); // done making snapshotJson

        snapshots[tick] = snapshotJson;

        if (snapshots.Count >= 20) // We store 20 frames in memory; the oldest frames are flushed.
        {
            WriteSnapshotsToDiskAndFlush();
        }
    }

    private void WriteSnapshotsToDiskAndFlush()
    {

        // Two functions:
        // 1. Write old snapshot to disk
        // 2. Flush unwanted data out of memory
        // Didn't check list legitimacy. default it.

        var bigDict = new Dictionary<int, string>();
        
        // 保存的Chunk两端的tick序数
        int startTick = snapshots.Keys.Min();
        int endTick = startTick + chunk_size - 1;

        // 填充chunk字典
        for (int i = startTick; i <= endTick; i++)
        {
            bigDict[i] = snapshots[i];
        }

        // 转为字符串格式并存储，此时无需清理snapshots，后续直接覆盖
        string bigJson = JsonUtility.ToJson(new IntKeyedSerializableDictionary(bigDict));
        string filePath = GetSnapshotFilePath(startTick, endTick);
        File.WriteAllText(filePath, bigJson);
        Debug.Log($"Wrote snapshots {startTick} to {endTick} to disk at {filePath}");

        // Flushing
        for (int i = startTick; i <= endTick; i++)
        {
            snapshots.Remove(i);
        }
    }

    private bool LoadSnapshot(int tickToLoad)
    {
        /* !!!!!!!!!!!!!!! tickToLoad!!! */
        if (snapshots.ContainsKey(tickToLoad)) // 内存中
        {
            string snapshotJson = snapshots[tickToLoad];
            ApplySnapshot(snapshotJson);


            List<int> keysToRemove = new List<int>();


            foreach (int key in snapshots.Keys)
            {
                if (key > tickToLoad) keysToRemove.Add(key);
            }
            foreach (int key in keysToRemove)
            {
                snapshots.Remove(key);
            }

            return true;
        }
        else
        {
            int startTick = tickToLoad - tickToLoad % chunk_size;
            int endTick = startTick + chunk_size - 1;

            string filePath = GetSnapshotFilePath(startTick, endTick);
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"No snapshot file found for tick !");
                return false;
            }

            string bigJson = File.ReadAllText(filePath);
            Debug.Log("Reading snapshot file: " + bigJson);
            IntKeyedSerializableDictionary fileDict = JsonUtility.FromJson<IntKeyedSerializableDictionary>(bigJson);
            Dictionary<int, string> bigDict = fileDict.ToDictionary();
            
            bigDict.TryGetValue(tickToLoad, out string snapshotJson);
            
            ApplySnapshot(snapshotJson);

            foreach (var kvp in bigDict)
            {
                int tick = kvp.Key;
                string snapshot = kvp.Value;
                snapshots[tick] = snapshot;
            }

            List<int> keysToRemove = new List<int>();

            foreach (int key in snapshots.Keys)
            {
                if (key > tickToLoad) keysToRemove.Add(key);
            }
            foreach (int key in keysToRemove)
            {
                snapshots.Remove(key);
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


    private bool canGoToNextTick = false;

    public void NotifyMobileUpdate()
    {
        canGoToNextTick = true;
    }
}
