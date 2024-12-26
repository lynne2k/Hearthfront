using UnityEngine;

public class Switch : MonoBehaviour
{
    public Door door;  // 与开关绑定的门
    public LayerMask triggerLayer;  // 触发开关的物体层
    private SpriteRenderer switchRenderer;  // 用于显示开关的精灵

    // 公共变量，用于在编辑器中绑定两个不同状态的Sprite
    public Sprite activatedSwitchSprite;  // 激活状态（开）
    public Sprite deactivatedSwitchSprite;  // 未激活状态（关）

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
        // 检测当前位置是否有物体，如果有物体在开关位置，打开门，并激活开关
        if (IsObjectOnSwitch())
        {
            door.OpenDoor();  // 打开门
            switchRenderer.sprite = activatedSwitchSprite;  // 激活开关
        }
        else
        {
            door.CloseDoor();  // 关闭门
            switchRenderer.sprite = deactivatedSwitchSprite;  // 关闭开关
        }
    }

    // 检查是否有物体在开关位置
    private bool IsObjectOnSwitch()
    {
        // 使用 OverlapCircle 或者 OverlapBox 判断是否有物体在开关的触发区域内
        // 这里用的是 `OverlapCircle`，检查开关周围一定范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, triggerLayer);

        return colliders.Length > 0;  // 如果检测到任何物体，则返回 true
    }
}
