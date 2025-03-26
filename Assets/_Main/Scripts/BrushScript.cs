using UnityEngine;

public class Brush : MonoBehaviour
{
    private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");
    
    public CustomRenderTexture heightMapRenderTexture;
    public CustomRenderTexture PaintingRenderTexture;
    public Material heightMapMaterial;
    public Material paintingMaterial;
    
    public Camera mainCamera;
    

    void Start()
    {
        mainCamera = Camera.main;
        heightMapRenderTexture.Initialize();
        heightMapRenderTexture.updateMode = CustomRenderTextureUpdateMode.Realtime;
        heightMapMaterial.SetVector(DrawPosition, new Vector4(-2, -2, 0, 0));
        
        PaintingRenderTexture.Initialize();
        PaintingRenderTexture.updateMode = CustomRenderTextureUpdateMode.Realtime;
        paintingMaterial.SetVector(DrawPosition, new Vector4(-2, -2, 0, 0));
    }

    private void Update()
    {
        if (GameManager.instance.CurrentTool == GameManager.Tool.Shaping)
        {
           if (Input.GetMouseButton(0)) 
           {
               Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
           
               if (Physics.Raycast(ray, out RaycastHit hit))
               {
                   Vector2 hitTextureCoord = hit.textureCoord;
                           
                   heightMapMaterial.SetVector(DrawPosition, new Vector4(hitTextureCoord.x, hitTextureCoord.y, 0, 0));
                           
                   heightMapRenderTexture.Update();
               }
           }
           else
           {
               heightMapMaterial.SetVector(DrawPosition, new Vector4(-2, -2, 0, 0));
           } 
        }
        else if (GameManager.instance.CurrentTool == GameManager.Tool.Painting)
        {
            if (Input.GetMouseButton(0)) 
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
           
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector2 hitTextureCoord = hit.textureCoord;
                           
                    paintingMaterial.SetVector(DrawPosition, new Vector4(hitTextureCoord.x, hitTextureCoord.y, 0, 0));
                           
                    PaintingRenderTexture.Update();
                }
            }
            else
            {
                paintingMaterial.SetVector(DrawPosition, new Vector4(-2, -2, 0, 0));
            } 
        }
        
        
    }
}