using UnityEngine;

public class Switch : MonoBehaviour
{
    public Door door;  // �뿪�ذ󶨵���
    public LayerMask triggerLayer;  // �������ص������
    private SpriteRenderer switchRenderer;  // ������ʾ���صľ���

    // ���������������ڱ༭���а�������ͬ״̬��Sprite
    public Sprite activatedSwitchSprite;  // ����״̬������
    public Sprite deactivatedSwitchSprite;  // δ����״̬���أ�

    private void Start()
    {
        // ��ȡSpriteRenderer�����ȷ��������SpriteRenderer
        switchRenderer = GetComponent<SpriteRenderer>();
        if (switchRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Switch object. Please add one.");
            return;
        }

        // ��ʼʱ���ÿ��ص�״̬
        switchRenderer.sprite = deactivatedSwitchSprite;  // Ĭ����δ����״̬
    }

    private void Update()
    {
        // ��⵱ǰλ���Ƿ������壬����������ڿ���λ�ã����ţ��������
        if (IsObjectOnSwitch())
        {
            door.OpenDoor();  // ����
            switchRenderer.sprite = activatedSwitchSprite;  // �����
        }
        else
        {
            door.CloseDoor();  // �ر���
            switchRenderer.sprite = deactivatedSwitchSprite;  // �رտ���
        }
    }

    // ����Ƿ��������ڿ���λ��
    private bool IsObjectOnSwitch()
    {
        // ʹ�� OverlapCircle ���� OverlapBox �ж��Ƿ��������ڿ��صĴ���������
        // �����õ��� `OverlapCircle`����鿪����Χһ����Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, triggerLayer);

        return colliders.Length > 0;  // �����⵽�κ����壬�򷵻� true
    }
}
