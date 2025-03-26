using System;
using System.Collections;
using System.Collections.Generic;
using TextureCompare;
using UnityEngine;
using UnityEngine.UI;
using Plane = _Main.Scripts.Plane;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public TMP_Dropdown ToolDropdown;
    public Slider ShapingSlider;
    public TMP_Dropdown ColorDropdown;
    
    public Tool CurrentTool { get; private set; }
    
    public Plane Plane;
    
    public Color[] Colors =
    {
        new(0,0,0,1),
        new(1,1,1,1),
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ToolDropdown.onValueChanged.AddListener(delegate { SetCurrentTool((Tool) ToolDropdown.value); });
        SetCurrentTool(Tool.Shaping);
        
        ColorDropdown.onValueChanged.AddListener(delegate { Plane.PaintMaterial.SetColor("_BrushColor",Colors[ColorDropdown.value]); });
        Plane.PaintMaterial.SetColor("_BrushColor",Colors[0]);
    }

    private void Update()
    {
        if (Plane != null)
        {
            Plane.HeightMapMaterial.SetColor("_BrushColor",Color.Lerp(Color.black, Color.white, ShapingSlider.value));
        }
    }

    public enum Tool
    {
        Shaping,
        Painting,
    }

    public void SetCurrentTool(Tool tool)
    {
        CurrentTool = tool;
        switch (tool)
        {
            case Tool.Shaping:
                ColorDropdown.gameObject.SetActive(false);
                ShapingSlider.gameObject.SetActive(true);
                break;
            case Tool.Painting:
                ColorDropdown.gameObject.SetActive(true);
                ShapingSlider.gameObject.SetActive(false);
                break;
        }
    }

    public void Grade()
    {
        var heightGrade = TextureComparer.CompareTextures(RenderTextureToTexture2D(Plane.GetComponent<Brush>().heightMapRenderTexture), Texture2D.blackTexture, TextureComparer.DifficultyLevel.Medium);
        Debug.Log("height grade is " + heightGrade);
    }

    public Texture2D RenderTextureToTexture2D(RenderTexture renderTexture)
    {
        RenderTexture currentRT = RenderTexture.active;
        
        Texture2D tex = new Texture2D(renderTexture.width,renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();
        
        RenderTexture.active = currentRT;
        return tex;
    }
}
