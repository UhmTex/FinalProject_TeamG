using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetPlane : MonoBehaviour
{
    [FormerlySerializedAs("PaintTexture")]
    public CustomRenderTexture PaintRenderTexture;
    public CustomRenderTexture HeightMapRenderTexture;
    
    [HideInInspector]
    public Texture2D HeightMapTexture;
    [HideInInspector]
    public Texture2D PaintTexture;
    
    public List<Texture2D> PaintTextures;
    public List<Texture2D> HeightMapTextures;

    private void Start()
    {
        GameManager.instance.TargetPlane = this;
        
        if (PaintTextures.Count > 0)
        {
            PaintTexture = PaintTextures[UnityEngine.Random.Range(0, PaintTextures.Count)];
            
            PaintRenderTexture.initializationTexture = PaintTexture;
            PaintRenderTexture.Initialize();
            PaintRenderTexture.updateMode = CustomRenderTextureUpdateMode.Realtime;
            
        }

        if (HeightMapTextures.Count > 0)
        {
            HeightMapTexture = HeightMapTextures[UnityEngine.Random.Range(0, HeightMapTextures.Count)];
            
            HeightMapRenderTexture.initializationTexture = HeightMapTexture;
            HeightMapRenderTexture.Initialize();
            HeightMapRenderTexture.updateMode = CustomRenderTextureUpdateMode.Realtime;
        }
        
        gameObject.SetActive(false);
    }
}
