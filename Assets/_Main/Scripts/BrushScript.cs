using UnityEngine;
using UnityEngine.VFX;

public class Brush : MonoBehaviour
{
    private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");
    
    public CustomRenderTexture heightMapRenderTexture;
    public CustomRenderTexture PaintingRenderTexture;
    public Material heightMapMaterial;
    public Material paintingMaterial;

    [SerializeField]
    private VisualEffect GrassCuttingVFX;

    [SerializeField]
    private VisualEffect SparklesVFX;

    [SerializeField]
    private VisualEffect SprayVFX;

    [SerializeField]
    private VisualEffect CircleVFX;

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

    public Texture2D RenderTextureToTexture2D(RenderTexture renderTexture)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;

        return tex;
    }

    private void Update()
    {
        if (GameManager.instance.CurrentTool == GameManager.Tool.Shaping)
        {
            Ray CirclePlacementRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(CirclePlacementRay, out RaycastHit objHit))
            {
                CircleVFX.transform.position = objHit.point + new Vector3(0,0.3f,0);
                CircleVFX.enabled = true;
            }

           if (Input.GetMouseButton(0)) 
           {
               Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
           
               if (Physics.Raycast(ray, out RaycastHit hit))
               {

                    GrassCuttingVFX.transform.position = hit.point + new Vector3(0,1,0);
                    SparklesVFX.transform.position = hit.point + new Vector3(0,1,0);

                    Vector2 hitTextureCoord = hit.textureCoord;

                    if (Input.GetMouseButtonDown(0))
                    {
                        Texture2D checkTex = RenderTextureToTexture2D(heightMapRenderTexture);

                        int texelX = Mathf.FloorToInt(hitTextureCoord.x * checkTex.width);
                        int texelY = Mathf.FloorToInt(hitTextureCoord.y * checkTex.height);

                        Color checkedColor = checkTex.GetPixel(texelX, texelY, 0);

                        Color brushColor = heightMapMaterial.GetColor("_BrushColor");

                        if (checkedColor.r < brushColor.r)
                        {
                            SparklesVFX.enabled = true;
                        }
                        else if (checkedColor.r == brushColor.r)
                        {
                            SparklesVFX.enabled = false;
                            GrassCuttingVFX.enabled = false;
                        }
                        else
                        {
                            SparklesVFX.enabled = false;
                            GrassCuttingVFX.enabled = true;
                        }
                    }                    
                           
                    heightMapMaterial.SetVector(DrawPosition, new Vector4(hitTextureCoord.x, hitTextureCoord.y, 0, 0));
                           
                    heightMapRenderTexture.Update();
               }
           }
           else
           {                
                SparklesVFX.enabled = false;
                GrassCuttingVFX.enabled = false;
                heightMapMaterial.SetVector(DrawPosition, new Vector4(-2, -2, 0, 0));
           } 
        }
        else if (GameManager.instance.CurrentTool == GameManager.Tool.Painting)
        {
            CircleVFX.enabled = false;

            if (Input.GetMouseButton(0)) 
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
           
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector2 hitTextureCoord = hit.textureCoord;
                           
                    paintingMaterial.SetVector(DrawPosition, new Vector4(hitTextureCoord.x, hitTextureCoord.y, 0, 0));
                           
                    PaintingRenderTexture.Update();

                    SprayVFX.enabled = true;

                    SprayVFX.SetVector3("Target", hit.point + new Vector3(0,0.1f,0));
                }
            }
            else
            {
                SprayVFX.enabled = false;
                paintingMaterial.SetVector(DrawPosition, new Vector4(-2, -2, 0, 0));
            } 
        }
        
        
    }
}