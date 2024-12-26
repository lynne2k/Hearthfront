using UnityEngine;

public class DemoTrain : Mobile
{
    public Track track;  // �������
    private int currentTrackIndex = 0;  // ��ǰ����������
    private bool movingForward = true;  // ���Ƿ���ǰ��
    private SpriteRenderer trainRenderer;  // ������ʾ�𳵵ľ���

    // ���������������ڱ༭���а�������ͬ״̬��Sprite
    public Sprite activatedSprite;  // �𳵼���״̬�ľ���
    public Sprite deactivatedSprite;  // ��δ����״̬�ľ���

    // ��¼�Ƿ����ڸ���
    public override void OnPossess()
    {
        isPossessed = true;
        trainRenderer.sprite = activatedSprite;  // ������ʱ��ʹ�ü���״̬�ľ���
    }

    // �������
    public override void OnUnpossess()
    {
        isPossessed = false;
        trainRenderer.sprite = deactivatedSprite;  // ��δ����ʱ��ʹ��δ����״̬�ľ���
    }

    void Start()
    {
        if (track == null || track.GetTrackLength() < 2)
        {
            Debug.LogError("Track must be assigned and have at least 2 points!");
            return;
        }

        // ��ȡ�𳵵� SpriteRenderer
        trainRenderer = GetComponent<SpriteRenderer>();
        if (trainRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Train object. Please add one.");
            return;
        }

        // ���𳵳�ʼλ������Ϊ�������ʼ��
        transform.position = track.GetPoint(currentTrackIndex);
        trainRenderer.sprite = deactivatedSprite;  // ��ʼʱ����Ϊδ����״̬
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
        // ���Ŀ��λ���Ƿ��������赲 
        int next_index = movingForward ? currentTrackIndex + 1 : currentTrackIndex - 1;
        Debug.Log($"��һ֡��{(next_index + track.GetTrackLength()) % track.GetTrackLength()}");
        Vector3 targetPosition = track.GetPoint((next_index + track.GetTrackLength()) % track.GetTrackLength());
        if (CanMoveTo(targetPosition))  // �Ƚ�����ײ���
        {
            // ���Ŀ��λ��û����ײ�壬�ͽ���ǰ�������
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
        if ((currentTrackIndex < track.GetTrackLength() - 1 || (track.isLoop && currentTrackIndex == track.GetTrackLength() - 1)) && isPossessed)
        {
            currentTrackIndex = (currentTrackIndex + 1) % track.GetTrackLength();  // ѭ�����
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
        if ((currentTrackIndex > 0 || (track.isLoop && currentTrackIndex == 0)) && isPossessed)
        {
            currentTrackIndex = (currentTrackIndex - 1 + track.GetTrackLength()) % track.GetTrackLength();  // ѭ�����
            transform.position = track.GetPoint(currentTrackIndex);
        }
        else
        {
            Debug.Log("Train has reached the beginning of the track!");
        }
    }

    // ���Ŀ��λ���Ƿ�����ƶ���
    private bool CanMoveTo(Vector3 targetPosition)
    {
        // ʹ�� OverlapCircle �����Ŀ��λ���Ƿ��������赲
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPosition, 0.2f);

        // ����������赲������ false�����û���赲���򷵻� true
        return colliders.Length == 0;
    }
}

[System.Serializable]
public class TrainData
{
    public int currentIndex;
    public bool isMovingForward;
}
