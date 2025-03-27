
using TextureCompare;
using UnityEngine;
using UnityEngine.UI;
using Plane = _Main.Scripts.Plane;
using TMPro;
using UnityEditor;
using UnityEngine.VFX;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(1)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public TMP_Dropdown ToolDropdown;
    public Slider ShapingSlider;
    public TMP_Dropdown ColorDropdown;
    public TMP_Dropdown DifficultyDropdown;

    public TMP_Text ScoreText;
    
    public Slider BushSizeSlider;
    
    public Tool CurrentTool { get; private set; }
    
    public Plane Plane;
    public TargetPlane TargetPlane;

    private Color[] _colors;

    private bool GameEnded;

    [SerializeField]
    private VisualEffect CircleVFX;

    [SerializeField]
    private VisualEffect SprayVFX;


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
        
        ColorDropdown.onValueChanged.AddListener(delegate { 
            Plane.PaintMaterial.SetColor("_BrushColor",_colors[ColorDropdown.value]);
            SprayVFX.SetVector4("SprayColor", _colors[ColorDropdown.value] * 50);
        });
        Plane.PaintMaterial.SetColor("_BrushColor",_colors[0]);
        SprayVFX.SetVector4("SprayColor", _colors[0] * 50);
    }

    private void Update()
    {
        if (Plane != null)
        {
            Plane.HeightMapMaterial.SetColor("_BrushColor",Color.Lerp(Color.black, Color.white, ShapingSlider.value));
            
            var val = BushSizeSlider.value;
            Plane.PaintMaterial.SetFloat("_BrushSize", val);
            Plane.HeightMapMaterial.SetFloat("_BrushSize", val);

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Plane.gameObject.SetActive(false);
                TargetPlane.gameObject.SetActive(true);
            }            

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                Plane.gameObject.SetActive(true);
                TargetPlane.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.F) && GameEnded)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
            }

            CircleVFX.SetFloat("CircleRadius", val * 10);
        }
    }
    
    public void ResetCircleVFX()
    {
        CircleVFX.enabled = false;
        CircleVFX.enabled = true;
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
        var heightGrade = TextureComparer.CompareTextures(RenderTextureToTexture2D(Plane.HeightMapTexture), TargetPlane.HeightMapTexture, (TextureComparer.DifficultyLevel)DifficultyDropdown.value);
        var paint = TextureComparer.CompareTextures(RenderTextureToTexture2D(Plane.PaintTexture), TargetPlane.PaintTexture, (TextureComparer.DifficultyLevel)DifficultyDropdown.value);
        
        float grade = (heightGrade + paint) / 2;
        Debug.Log(" grade is " + grade);

        ScoreText.enabled = true;
        ScoreText.text = $"Your Grade Is: {(int)(grade*100)}\n\nPress F To Restart";

        GameEnded = true;
    }

    [ContextMenu("Save Textures")]
    public void SaveTextures()
    {
        // Convert HeightMap RenderTexture to Texture2D
        Texture2D heightTexture = RenderTextureToTexture2D(Plane.HeightMapTexture);

        // Save the HeightMap texture as an asset
        byte[] heightBytes = heightTexture.EncodeToPNG();
        string heightPath = $"Assets/_Main/PreMade/HeightMapTexture_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        System.IO.File.WriteAllBytes(heightPath, heightBytes);
        AssetDatabase.ImportAsset(heightPath);

        // Convert Paint RenderTexture to Texture2D
        Texture2D paintTexture = RenderTextureToTexture2D(Plane.PaintTexture);

        // Save the Paint texture as an asset
        byte[] paintBytes = paintTexture.EncodeToPNG();
        string paintPath = $"Assets/_Main/PreMade/PaintTexture_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        System.IO.File.WriteAllBytes(paintPath, paintBytes);
        AssetDatabase.ImportAsset(paintPath);

        Debug.Log("Textures saved to Assets/SavedTextures/");
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
