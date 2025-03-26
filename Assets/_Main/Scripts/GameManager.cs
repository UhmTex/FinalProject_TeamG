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
        var heightGrade = TextureComparer.CompareTextures(RenderTextureToTexture2D(Plane.GetComponent<Brush>().renderTexture), Texture2D.blackTexture, TextureComparer.DifficultyLevel.Medium);
        Debug.Log("height grade is " + heightGrade);
    }

    public Texture2D RenderTextureToTexture2D(RenderTexture renderTexture)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Plane.GetComponent<Brush>().renderTexture;
        
        Texture2D planeHeight = new Texture2D(Plane.GetComponent<Brush>().renderTexture.width, Plane.GetComponent<Brush>().renderTexture.height, TextureFormat.RGBA32, false);
        planeHeight.ReadPixels(new Rect(0, 0, planeHeight.width, planeHeight.height), 0, 0);
        planeHeight.Apply();
        
        RenderTexture.active = currentRT;
        return planeHeight;
    }
}
