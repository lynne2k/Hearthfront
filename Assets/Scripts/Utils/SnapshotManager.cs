using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class SnapshotManager
{
    // �����snapshotInterval��Ҫ���ⲿ�����������Ϊ��̬ȫ�ֱ���
    public static int snapshotInterval = 5;

    // д���tick���յ�Ӳ��
    public static void WriteSnapshotsToDisk(List<string> snapshots, int startTick, int endTick, string baseDir)
    {
        var bigDict = new Dictionary<int, string>();
        int intervalCount = endTick - startTick + 1;
        for (int i = 0; i < intervalCount; i++)
        {
            int tick = startTick + i;
            bigDict[tick] = snapshots[i];
        }

        string bigJson = JsonUtility.ToJson(new IntKeyedSerializableDictionary(bigDict));
        string filePath = GetSnapshotFilePath(startTick, endTick, baseDir);
        File.WriteAllText(filePath, bigJson);
        Debug.Log($"Wrote snapshots {startTick} to {endTick} to disk at {filePath}");
    }

    public static string GetSnapshotFilePath(int startTick, int endTick, string baseDir)
    {
        string dir = Path.Combine(baseDir, "Snapshots");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return Path.Combine(dir, $"snapshot_{startTick}_{endTick}.json");
    }

    // ���ڴ���ļ��м���ָ��tick�Ŀ���
    // ע��������Ҫ֪��currentTick, snapshots��baseDir���Լ�allMobiles������
    public static bool LoadSnapshot(
        int tickToLoad,
        int currentTick,
        List<string> snapshots,
        Mobile[] allMobiles,
        int snapshotInterval,
        string baseDir
    )
    {
        int oldestInMemoryTick = currentTick - snapshots.Count + 1;
        if (tickToLoad >= oldestInMemoryTick && tickToLoad <= currentTick)
        {
            // �ڴ���
            int index = tickToLoad + 1 - oldestInMemoryTick;
            string snapshotJson = snapshots[index];
            ApplySnapshot(snapshotJson, allMobiles);
            return true;
        }
        else
        {
            // �ļ���
            
            int chunkIndex = (tickToLoad + 1) / snapshotInterval;
            int startTick = chunkIndex * snapshotInterval + 1;
            int endTick = startTick + snapshotInterval - 1;
            Debug.Log($"chunkIndex:{chunkIndex} startTick:{startTick} endTick: {endTick}");

            string filePath = GetSnapshotFilePath(startTick, endTick, baseDir);
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"No snapshot file found for tick {tickToLoad}!");
                return false;
            }

            string bigJson = File.ReadAllText(filePath);
            IntKeyedSerializableDictionary fileDict = JsonUtility.FromJson<IntKeyedSerializableDictionary>(bigJson);
            Dictionary<int, string> bigDict = fileDict.ToDictionary();
            if (bigDict.TryGetValue(tickToLoad, out string snapshotJson))
            {
                ApplySnapshot(snapshotJson, allMobiles);
                return true;
            }
            else
            {
                Debug.LogWarning($"Tick {tickToLoad} not found in file {filePath}!");
                return false;
            }
        }
    }

    public static void ApplySnapshot(string snapshotJson, Mobile[] allMobiles)
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
        Debug.Log("Snapshot applied.");
    }
}
