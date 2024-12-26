using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTrain : Mobile
{
    public Track track;  // �������
    private int currentTrackIndex = 0;  // ��ǰ����������
    private bool movingForward = true;  // ���Ƿ���ǰ��
    private float moveSpeed = 2f;      // �𳵵��ƶ��ٶ�

    // ��¼�Ƿ����ڸ���
    public override void OnPossess()
    {
        isPossessed = true;
    }

    // �������
    public override void OnUnpossess()
    {
        isPossessed = false;
    }

    void Start()
    {
        if (track == null || track.GetTrackLength() < 2)
        {
            Debug.LogError("Track must be assigned and have at least 2 points!");
            return;
        }

        // ���𳵳�ʼλ������Ϊ�������ʼ��
        transform.position = track.GetPoint(currentTrackIndex);
    }

    public override string Save()
    {
        TrainData data = new TrainData
        {
            currentIndex = currentTrackIndex,
            isMovingForward = movingForward
        };

        return JsonUtility.ToJson(data);
    }

    public override void Load(string loadedData)
    {
        TrainData data = JsonUtility.FromJson<TrainData>(loadedData);
        currentTrackIndex = data.currentIndex;
        movingForward = data.isMovingForward;

        transform.position = track.GetPoint(currentTrackIndex);
    }

    public override void OnTick(int tick)
    {
        // ���յ�ǰ������ǰ������ƶ�
        if (movingForward && isPossessed)
        {
            MoveForward();
        }
        else if (!movingForward && isPossessed)
        {
            MoveBackward();
        }
    }

    void Update()
    {
        // ��ҿ��ƻ�ǰ�������
        if (Input.GetKeyDown(KeyCode.W))  // W ������ǰ��
        {
            movingForward = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))  // S �����ƺ���
        {
            movingForward = false;
        }
    }

    // ��ǰ�ƶ�һ��
    private void MoveForward()
    {
        if (currentTrackIndex < track.GetTrackLength() - 1)
        {
            currentTrackIndex++;
            transform.position = track.GetPoint(currentTrackIndex);
        }
        else
        {
            Debug.Log("Train has reached the end of the track!");
        }
    }

    // ����ƶ�һ��
    private void MoveBackward()
    {
        if (currentTrackIndex > 0)
        {
            currentTrackIndex--;
            transform.position = track.GetPoint(currentTrackIndex);
        }
        else
        {
            Debug.Log("Train has reached the beginning of the track!");
        }
    }
}

[System.Serializable]
public class TrainData
{
    public int currentIndex;
    public bool isMovingForward;
}
