using UnityEngine;

public static class GridUtils
{
    /// 将传入的 Vector3 向最近的网格点取整。
    public static Vector3 RoundVector3(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y),
            Mathf.Round(vector.z)
        );
    }

    /// 将传入的 Vector3 向最近的网格点取整，并返回 Vector3Int 类型。
    public static Vector3Int RoundVector3Int(Vector3 vector)
    {
        var roundedVector = RoundVector3(vector);
        return new Vector3Int(
            (int)roundedVector.x,
            (int)roundedVector.y,
            (int)roundedVector.z
        );
    }

    /// 翻转方向（静态方法）
    public static void FlipDirection(Transform transform, ref bool facingRight)
    {
        facingRight = !facingRight; // 切换朝向状态

        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // 翻转 X 轴缩放
        transform.localScale = theScale;
    }
}
