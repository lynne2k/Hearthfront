using UnityEngine;

public class Switch : MonoBehaviour
{
    public Door door;  // 与开关绑定的门
    public LayerMask triggerLayer;  // 触发开关的物体层
    private SpriteRenderer switchRenderer;  // 用于显示开关的精灵

    // 公共变量，用于在编辑器中绑定两个不同状态的Sprite
    public Sprite activatedSwitchSprite;  // 激活状态（开）
    public Sprite deactivatedSwitchSprite;  // 未激活状态（关）

    public bool isPressed = false;

    private void Start()
    {
        // 获取SpriteRenderer组件，确保开关有SpriteRenderer
        switchRenderer = GetComponent<SpriteRenderer>();
        if (switchRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Switch object. Please add one.");
            return;
        }

        // 初始时设置开关的状态
        switchRenderer.sprite = deactivatedSwitchSprite;  // 默认是未激活状态
    }

    private void Update()
    {
        isPressed = IsObjectOnSwitch();
    }

    // 检查是否有物体在开关位置
    private bool IsObjectOnSwitch()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f, triggerLayer);

        return colliders.Length > 0;
    }
}