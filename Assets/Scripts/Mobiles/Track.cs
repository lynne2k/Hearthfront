using UnityEngine;
using System.Collections.Generic;

public class Track : MonoBehaviour
{
    public List<Transform> trackPoints;  // �洢������ Transform

    private void Start()
    {
        if (trackPoints.Count < 2)
        {
            Debug.LogError("����ϱ��������������㣡");
        }
    }

    // ��ȡ�����ĳ�����λ��
    public Vector3 GetPoint(int index)
    {
        return trackPoints[index].position;
    }

    // ��ȡ����ϵ��ܵ���
    public int GetTrackLength()
    {
        return trackPoints.Count;
    }
}
