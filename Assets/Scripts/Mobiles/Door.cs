using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;  // ���Ƿ���
    private SpriteRenderer doorRenderer; // ������ʾ�ŵľ���
    private Collider2D doorCollider;  // �ŵ���ײ��

    // ���������������ڱ༭���а�������ͬ״̬��Sprite
    public Sprite openDoorSprite;
    public Sprite closedDoorSprite;

    private void Start()
    {
        // ��ȡSpriteRenderer�����ȷ������SpriteRenderer
        doorRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();  // ��ȡ�ŵ���ײ�����

        if (doorRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Door object. Please add one.");
        }

        if (doorCollider == null)
        {
            Debug.LogError("Collider2D not found on Door object. Please add a Collider2D.");
        }

        // ��ʼʱ�����ŵ�״̬
        if (isOpen)
        {
            doorRenderer.sprite = openDoorSprite;
            doorCollider.enabled = false;  // ����ʱ������ײ��
        }
        else
        {
            doorRenderer.sprite = closedDoorSprite;
            doorCollider.enabled = true;   // ����ʱ������ײ��
        }
    }

    // ����
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            doorRenderer.sprite = openDoorSprite;  // ������Ϊ��״̬�ľ���
            doorCollider.enabled = false;  // ������ײ��
            Debug.Log("Door opened");
        }
    }

    // �ر���
    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            doorRenderer.sprite = closedDoorSprite;  // ������Ϊ�ر�״̬�ľ���
            doorCollider.enabled = true;   // ������ײ��
            Debug.Log("Door closed");
        }
    }
}
