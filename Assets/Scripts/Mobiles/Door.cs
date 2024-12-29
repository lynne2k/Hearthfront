using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;  // 门是否开启
    private SpriteRenderer doorRenderer; // 用于显示门的精灵
    private Collider2D doorCollider;  // 门的碰撞器

    // 公共变量，用于在编辑器中绑定两个不同状态的Sprite
    public Sprite[] sprites;

    public Switch[] switches;


    public int frameIndex = 0;
    private float animationFrameCooldown = 0f;

    private LayerMask mobileLayer = 1 << 11;

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
            frameIndex = 0;
            doorRenderer.sprite = sprites[0];
            doorCollider.enabled = false;  // 开门时禁用碰撞器
        }
        else
        {
            frameIndex = sprites.Length - 1;
            doorRenderer.sprite = sprites[sprites.Length - 1];
            doorCollider.enabled = true;   // 关门时启用碰撞器
        }
    }

    private void Update()
    {
        bool isAllButtonPressed = true;
        foreach (var sw in switches)
        {
            if (!sw.isPressed) isAllButtonPressed = false;
        }
        if (isAllButtonPressed && !isOpen)
        {
            OpenDoor();
        }
        else if (!isAllButtonPressed && isOpen)
        {
            CloseDoor();
        }
        
    }

    private void LateUpdate()
    {
        if (isOpen && frameIndex != 0 && animationFrameCooldown <= 0f)
        {
            frameIndex--;
            animationFrameCooldown = 0.1f;
        }
        else if (!isOpen && frameIndex != sprites.Length - 1 && animationFrameCooldown <= 0f && !checkCollision())
        {
            frameIndex++;
            animationFrameCooldown = 0.1f;
        }
        else
        {
            animationFrameCooldown -= Time.deltaTime;
            animationFrameCooldown = animationFrameCooldown <= 0f ? 0f : animationFrameCooldown;
        }

        doorRenderer.sprite = sprites[frameIndex];
    }



    private void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            doorCollider.enabled = false;  // 禁用碰撞器
            Debug.Log("Door opened");
        }
    }

    private void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            doorCollider.enabled = true;   // 启用碰撞器
            Debug.Log("Door closed");
        }
    }

    private bool checkCollision()
    {
        var gridPosition = GameUtils.RoundVector3(transform.position);
        if (Physics2D.OverlapArea(gridPosition - new Vector3(0.2f, 0.2f, 0), gridPosition + new Vector3(0.2f, 0.2f, 0), mobileLayer))
            return true;
        return false;
    }
}