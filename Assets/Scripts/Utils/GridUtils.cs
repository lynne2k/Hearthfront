using UnityEngine;

public static class GridUtils
{
    /// ������� Vector3 ������������ȡ����
    public static Vector3 RoundVector3(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y),
            Mathf.Round(vector.z)
        );
    }

    /// ������� Vector3 ������������ȡ���������� Vector3Int ���͡�
    public static Vector3Int RoundVector3Int(Vector3 vector)
    {
        var roundedVector = RoundVector3(vector);
        return new Vector3Int(
            (int)roundedVector.x,
            (int)roundedVector.y,
            (int)roundedVector.z
        );
    }

    /// ��ת���򣨾�̬������
    public static void FlipDirection(Transform transform, ref bool facingRight)
    {
        facingRight = !facingRight; // �л�����״̬

        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // ��ת X ������
        transform.localScale = theScale;
    }
}
