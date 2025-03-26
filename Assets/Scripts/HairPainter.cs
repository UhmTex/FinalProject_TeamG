using UnityEngine;

public class HairPainter : MonoBehaviour
{
    public Material BrushMaterial;
    public RenderTexture HairColorTexture;

    private static readonly int BrushUV = Shader.PropertyToID("_BrushUV");
    private static readonly int BrushColor = Shader.PropertyToID("_BrushColor");
    private static readonly int BrushRadius = Shader.PropertyToID("_BrushRadius");
    private static readonly int BrushOpacity = Shader.PropertyToID("_BrushOpacity");

    void Start()
    {
        // Temporarily set the active RenderTexture to our target
        RenderTexture activeRT = RenderTexture.active;
        RenderTexture.active = HairColorTexture;

        // Clear it to white
        GL.Clear(true, true, Color.white);

        // Restore the original active RenderTexture
        RenderTexture.active = activeRT;
    }

    public void Paint(Vector2 uv, Color color, float radius, float opacity)
    {
        // BrushMaterial.SetVector(BrushUV, uv);
        // BrushMaterial.SetColor(BrushColor, color);
        // BrushMaterial.SetFloat(BrushRadius, radius);
        // BrushMaterial.SetFloat(BrushOpacity, opacity);
        //
        // RenderTexture temp = RenderTexture.GetTemporary(HairColorTexture.width, HairColorTexture.height);
        // Graphics.Blit(HairColorTexture, temp);
        // Graphics.Blit(temp, HairColorTexture, BrushMaterial);
        // RenderTexture.ReleaseTemporary(temp);
    }
}