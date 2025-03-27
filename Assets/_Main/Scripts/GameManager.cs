
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
    
    public Slider BushSizeSlider;
    
    public Tool CurrentTool { get; private set; }
    
    public Plane Plane;

    private Color[] _colors;
    

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

        _colors = new[]
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.magenta,
            Color.cyan,
            Color.white,
            Color.black,
        };
    }
    
    public enum Tool
    {
        Shaping,
        Painting,
    }

    private void Start()
    {
        ToolDropdown.onValueChanged.AddListener(delegate { SetCurrentTool((Tool) ToolDropdown.value); });
        SetCurrentTool(Tool.Shaping);
        
        ColorDropdown.onValueChanged.AddListener(delegate { Plane.PaintMaterial.SetColor("_BrushColor",_colors[ColorDropdown.value]); });
        Plane.PaintMaterial.SetColor("_BrushColor",_colors[0]);
    }

    private void Update()
    {
        if (Plane != null)
        {
            Plane.HeightMapMaterial.SetColor("_BrushColor",Color.Lerp(Color.black, Color.white, ShapingSlider.value));
            
            var val = BushSizeSlider.value;
            Plane.PaintMaterial.SetFloat("_BrushSize", val);
            Plane.HeightMapMaterial.SetFloat("_BrushSize", val);
        }
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
        var heightGrade = TextureComparer.CompareTextures(RenderTextureToTexture2D(Plane.HeightMapTexture), Texture2D.blackTexture, TextureComparer.DifficultyLevel.Medium);
        var paint = TextureComparer.CompareTextures(RenderTextureToTexture2D(Plane.PaintTexture), Texture2D.blackTexture, TextureComparer.DifficultyLevel.Medium);
        
        float grade = (heightGrade + paint) / 2;
        Debug.Log(" grade is " + grade);
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
}
