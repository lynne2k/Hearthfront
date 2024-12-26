using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;  // 引入 UnityEditor 命名空间
#endif

public class Track : MonoBehaviour
{
    public List<Vector3> trackPoints;  // 存储轨道点的 Vector3
    public bool isLoop = false;  // 是否循环轨道

    private void OnDrawGizmos()
    {
        // 如果轨道点为空，退出
        if (trackPoints.Count < 2) return;

        // 可视化轨道点
        for (int i = 0; i < trackPoints.Count; i++)
        {
            // 绘制轨道点
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(trackPoints[i], 0.1f);

            // 连接轨道点
            if (i < trackPoints.Count - 1)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(trackPoints[i], trackPoints[i + 1]);
            }

            // 如果是循环轨道，连接最后一个点与第一个点
            if (isLoop && i == trackPoints.Count - 1)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(trackPoints[i], trackPoints[0]);
            }
        }
    }

    // 获取轨道上某个点的位置
    public Vector3 GetPoint(int index)
    {
        // 如果是循环轨道，访问超出范围的点时将访问第一个点
        if (isLoop)
        {
            index = index % trackPoints.Count;
        }

        return trackPoints[index];
    }

    // 获取轨道上的总点数
    public int GetTrackLength()
    {
        return trackPoints.Count;
    }

    // 在 Inspector 中可视化轨道点，便于编辑
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // 可视化轨道点的编辑操作，帮助开发者在场景视图中查看并修改轨道点
        for (int i = 0; i < trackPoints.Count; i++)
        {
            Handles.Label(trackPoints[i], $"Point {i}"); // 标记轨道点
        }
    }
#endif
}
