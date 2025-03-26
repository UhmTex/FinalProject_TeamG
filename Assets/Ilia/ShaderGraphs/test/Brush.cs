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
        renderTexture.updateMode = CustomRenderTextureUpdateMode.Realtime; // Включаем постоянное обновление
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Проверяем нажатие мыши
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 hitTextureCoord = hit.textureCoord; // Получаем координаты удара в текстуре
                
                heightMapMaterial.SetVector(DrawPosition, new Vector4(hitTextureCoord.x, hitTextureCoord.y, 0, 0));
                
                // Принудительно обновляем текстуру
                Debug.Log($"Координаты: {hitTextureCoord.x}, {hitTextureCoord.y}");
                renderTexture.Update();
            }
        }
    }
}