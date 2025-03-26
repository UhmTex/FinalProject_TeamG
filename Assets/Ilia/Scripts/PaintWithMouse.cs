using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintWithMouse : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    private static readonly int SplatMap = Shader.PropertyToID("_SplatMap");
    private static readonly int Coordinates = Shader.PropertyToID("_Coordinates");
    private static readonly int Strength = Shader.PropertyToID("_Strength");
    private static readonly int Size = Shader.PropertyToID("_Size");

    [SerializeField] private Camera cam;
    [SerializeField] private Shader paintShader;
    
    [SerializeField] [Range(1, 500)] private float size;
    [SerializeField] [Range(0, 1)] private float strength;
    
    private RenderTexture _splatMap;
    private Material _currentMaterial, _drawMaterial;
    private RaycastHit _hit;


    // Start is called before the first frame update
    void Start()
    {
        _drawMaterial = new Material(paintShader);
        _drawMaterial.SetVector(Color1, Color.red);

        _currentMaterial = GetComponent<MeshRenderer>().material;
        
        _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _currentMaterial.SetTexture(SplatMap, _splatMap);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                Debug.Log(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit));
                _drawMaterial.SetVector(Coordinates, new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat(Strength, strength);
                _drawMaterial.SetFloat(Size, size);

                RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatMap, temp);
                Graphics.Blit(temp, _splatMap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
}
