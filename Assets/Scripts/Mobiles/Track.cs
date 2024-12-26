using UnityEngine;
using System.Collections.Generic;

public class Track : MonoBehaviour
{
    public List<Transform> trackPoints;  // 存储轨道点的 Transform

    private void Start()
    {
        if (trackPoints.Count < 2)
        {
            Debug.LogError("轨道上必须至少有两个点！");
        }
    }

    // 获取轨道上某个点的位置
    public Vector3 GetPoint(int index)
    {
        return trackPoints[index].position;
    }

    // 获取轨道上的总点数
    public int GetTrackLength()
    {
        return trackPoints.Count;
    }
}
