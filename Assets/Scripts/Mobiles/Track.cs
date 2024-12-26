using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;  // ���� UnityEditor �����ռ�
#endif

public class Track : MonoBehaviour
{
    public List<Vector3> trackPoints;  // �洢������ Vector3
    public bool isLoop = false;  // �Ƿ�ѭ�����

    private void OnDrawGizmos()
    {
        // ��������Ϊ�գ��˳�
        if (trackPoints.Count < 2) return;

        // ���ӻ������
        for (int i = 0; i < trackPoints.Count; i++)
        {
            // ���ƹ����
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(trackPoints[i], 0.1f);

            // ���ӹ����
            if (i < trackPoints.Count - 1)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(trackPoints[i], trackPoints[i + 1]);
            }

            // �����ѭ��������������һ�������һ����
            if (isLoop && i == trackPoints.Count - 1)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(trackPoints[i], trackPoints[0]);
            }
        }
    }

    // ��ȡ�����ĳ�����λ��
    public Vector3 GetPoint(int index)
    {
        // �����ѭ����������ʳ�����Χ�ĵ�ʱ�����ʵ�һ����
        if (isLoop)
        {
            index = index % trackPoints.Count;
        }

        return trackPoints[index];
    }

    // ��ȡ����ϵ��ܵ���
    public int GetTrackLength()
    {
        return trackPoints.Count;
    }

    // �� Inspector �п��ӻ�����㣬���ڱ༭
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // ���ӻ������ı༭�����������������ڳ�����ͼ�в鿴���޸Ĺ����
        for (int i = 0; i < trackPoints.Count; i++)
        {
            Handles.Label(trackPoints[i], $"Point {i}"); // ��ǹ����
        }
    }
#endif
}
