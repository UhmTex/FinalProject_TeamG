using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanationScript : MonoBehaviour
{
    private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");
    
    public CustomRenderTexture CRT_Explanation;
    public Material M_ExplanationShaderMaterial;
    
    private Camera _mainCamera;
    void Start()
    {
        CRT_Explanation.Initialize();
        _mainCamera = Camera.main;
        M_ExplanationShaderMaterial.SetVector(DrawPosition, new Vector4(2,2,0,0));
    }

  
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
           Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

           if (Physics.Raycast(ray, out RaycastHit hit))
           {
                Vector2 hitTexCoord = hit.textureCoord;
                
                M_ExplanationShaderMaterial.SetVector(DrawPosition, hitTexCoord);
           }
        }
    }
}
