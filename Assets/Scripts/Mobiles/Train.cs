using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTrain : Mobile
{
    public Track track;  // 轨道对象
    private int currentTrackIndex = 0;  // 当前轨道点的索引
    private bool movingForward = true;  // 火车是否在前进
    private float moveSpeed = 2f;      // 火车的移动速度

    // 记录是否正在附身
    public override void OnPossess()
    {
        isPossessed = true;
    }

    // 解除附身
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

        // 将火车初始位置设置为轨道的起始点
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
        // 按照当前方向向前或向后移动
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
        // 玩家控制火车前进或后退
        if (Input.GetKeyDown(KeyCode.W))  // W 键控制前进
        {
            movingForward = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))  // S 键控制后退
        {
            movingForward = false;
        }
    }

    // 向前移动一步
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

    // 向后移动一步
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
