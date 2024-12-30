using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScanLinesEffect : MonoBehaviour
{
    public Material scanLinesMaterial;

    void OnRenderObject()
    {
        if (scanLinesMaterial != null)
        {
            // Apply the scan lines material to the entire screen
            GL.PushMatrix();
            scanLinesMaterial.SetPass(0);

            // Draw a full-screen quad
            GL.Begin(GL.QUADS);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1, -1, 0);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1, -1, 0);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1, 1, 0);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1, 1, 0);
            GL.End();
            GL.PopMatrix();
        }
    }
}
