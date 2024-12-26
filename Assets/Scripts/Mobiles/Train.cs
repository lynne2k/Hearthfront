using UnityEngine;

public class DemoTrain : Mobile
{
    public Track track;  // 轨道对象
    private int currentTrackIndex = 0;  // 当前轨道点的索引
    private bool movingForward = true;  // 火车是否在前进
    private SpriteRenderer trainRenderer;  // 用于显示火车的精灵

    // 公共变量，用于在编辑器中绑定两个不同状态的Sprite
    public Sprite activatedSprite;  // 火车激活状态的精灵
    public Sprite deactivatedSprite;  // 火车未激活状态的精灵

    // 记录是否正在附身
    public override void OnPossess()
    {
        isPossessed = true;
        trainRenderer.sprite = activatedSprite;  // 当附身时，使用激活状态的精灵
    }

    // 解除附身
    public override void OnUnpossess()
    {
        isPossessed = false;
        trainRenderer.sprite = deactivatedSprite;  // 当未附身时，使用未激活状态的精灵
    }

    void Start()
    {
        if (track == null || track.GetTrackLength() < 2)
        {
            Debug.LogError("Track must be assigned and have at least 2 points!");
            return;
        }

        // 获取火车的 SpriteRenderer
        trainRenderer = GetComponent<SpriteRenderer>();
        if (trainRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Train object. Please add one.");
            return;
        }

        // 将火车初始位置设置为轨道的起始点
        transform.position = track.GetPoint(currentTrackIndex);
        trainRenderer.sprite = deactivatedSprite;  // 初始时设置为未激活状态
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
        // 检查目标位置是否有物体阻挡 
        int next_index = movingForward ? currentTrackIndex + 1 : currentTrackIndex - 1;
        Debug.Log($"下一帧是{(next_index + track.GetTrackLength()) % track.GetTrackLength()}");
        Vector3 targetPosition = track.GetPoint((next_index + track.GetTrackLength()) % track.GetTrackLength());
        if (CanMoveTo(targetPosition))  // 先进行碰撞检测
        {
            // 如果目标位置没有碰撞体，就进行前进或后退
            if (movingForward)
            {
                MoveForward();
            }
            else
            {
                MoveBackward();
            }
        }
        else
        {
            Debug.Log("Train cannot move due to obstruction!");
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
        if ((currentTrackIndex < track.GetTrackLength() - 1 || (track.isLoop && currentTrackIndex == track.GetTrackLength() - 1)) && isPossessed)
        {
            currentTrackIndex = (currentTrackIndex + 1) % track.GetTrackLength();  // 循环轨道
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
        if ((currentTrackIndex > 0 || (track.isLoop && currentTrackIndex == 0)) && isPossessed)
        {
            currentTrackIndex = (currentTrackIndex - 1 + track.GetTrackLength()) % track.GetTrackLength();  // 循环轨道
            transform.position = track.GetPoint(currentTrackIndex);
        }
        else
        {
            Debug.Log("Train has reached the beginning of the track!");
        }
    }

    // 检查目标位置是否可以移动到
    private bool CanMoveTo(Vector3 targetPosition)
    {
        // 使用 OverlapCircle 来检查目标位置是否有物体阻挡
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPosition, 0.2f);

        // 如果有物体阻挡，返回 false；如果没有阻挡，则返回 true
        return colliders.Length == 0;
    }
}

[System.Serializable]
public class TrainData
{
    public int currentIndex;
    public bool isMovingForward;
}
