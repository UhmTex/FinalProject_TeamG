using UnityEngine;

public class Brush : MonoBehaviour
{
    private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");
    
    public CustomRenderTexture renderTexture;
    public Material heightMapMaterial;
    
    public Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        renderTexture.Initialize();
        renderTexture.updateMode = CustomRenderTextureUpdateMode.Realtime;
        heightMapMaterial.SetVector(DrawPosition, new Vector4(-2, -2, 0, 0));
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 hitTextureCoord = hit.textureCoord;
                
                heightMapMaterial.SetVector(DrawPosition, new Vector4(hitTextureCoord.x, hitTextureCoord.y, 0, 0));
                
                renderTexture.Update();
            }
        }
    }
}