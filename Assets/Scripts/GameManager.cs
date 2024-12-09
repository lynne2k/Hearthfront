using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public const int framePerTick = 30;
    public int currentTick = 0;
    public int currentTickTimeDelta = 0;

    public int varFoo = 0;
    public string varBar = "Player";
    public Vector3Int varTestCoord = new Vector3Int(1, 3, 3);

    public List<string> snapshots = new List<string>();
    public Mobile[] allMobiles;

    private const int snapshotInterval = 30;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SnapshotManager.snapshotInterval = snapshotInterval; // 同步interval到SnapshotManager

        allMobiles = FindObjectsOfType<Mobile>();

        // 设置FixedUpdate的调用频率为30Hz
        Time.fixedDeltaTime = 1f / 30f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int tickToLoad = currentTick - 1;
            bool loaded = SnapshotManager.LoadSnapshot(
                tickToLoad,
                currentTick,
                snapshots,
                allMobiles,
                snapshotInterval,
                Application.persistentDataPath);

            if (loaded)
            {
                currentTick = tickToLoad;
                currentTickTimeDelta = 0;
                if (snapshots.Count > 0)
                {
                    snapshots.RemoveAt(snapshots.Count - 1);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        currentTickTimeDelta++;
        if ((currentTickTimeDelta + 1) % framePerTick == 0)
        {
            currentTick++;
            TakeSnapshot();

            foreach (Mobile mob in allMobiles)
            {
                mob.onTick(currentTick);
            }
        }
    }

    private void TakeSnapshot()
    {
        var snapshotData = new Dictionary<string, string>();
        foreach (var mob in allMobiles)
        {
            string json = mob.Save();
            snapshotData[mob.name] = json;
        }

        string snapshotJson = JsonUtility.ToJson(new SerializableDictionary(snapshotData));
        snapshots.Add(snapshotJson);

        if (currentTick % snapshotInterval == 0)
        {
            int startTick = currentTick - snapshotInterval + 1;
            int endTick = currentTick;

            SnapshotManager.WriteSnapshotsToDisk(snapshots, startTick, endTick, Application.persistentDataPath);

            // 移除已写入硬盘的快照
            snapshots.RemoveRange(0, snapshotInterval);
        }
    }
}
