using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;  // 门是否开启
    private SpriteRenderer doorRenderer; // 用于显示门的精灵
    private Collider2D doorCollider;  // 门的碰撞器

    // 公共变量，用于在编辑器中绑定两个不同状态的Sprite
    public Sprite openDoorSprite;
    public Sprite closedDoorSprite;

    private void Start()
    {
        // 获取SpriteRenderer组件，确保门有SpriteRenderer
        doorRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();  // 获取门的碰撞器组件

        if (doorRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Door object. Please add one.");
        }

        if (doorCollider == null)
        {
            Debug.LogError("Collider2D not found on Door object. Please add a Collider2D.");
        }

        // 初始时设置门的状态
        if (isOpen)
        {
            doorRenderer.sprite = openDoorSprite;
            doorCollider.enabled = false;  // 开门时禁用碰撞器
        }
        else
        {
            doorRenderer.sprite = closedDoorSprite;
            doorCollider.enabled = true;   // 关门时启用碰撞器
        }
    }

    // 打开门
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            doorRenderer.sprite = openDoorSprite;  // 设置门为打开状态的精灵
            doorCollider.enabled = false;  // 禁用碰撞器
            Debug.Log("Door opened");
        }
    }

    // 关闭门
    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            doorRenderer.sprite = closedDoorSprite;  // 设置门为关闭状态的精灵
            doorCollider.enabled = true;   // 启用碰撞器
            Debug.Log("Door closed");
        }
    }
}
