using UnityEngine;

public class BrushController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private HairPainter _texturePainter;

    [Header("Brush Settings")] [SerializeField]
    private BrushColorPreset _colorPreset;

    [SerializeField] private float _brushRadius = 0.05f;
    [SerializeField, Range(0f, 1f)] private float _opacity = 1f;

    private static readonly int BrushRadiusID = Shader.PropertyToID("_BrushRadius");
    private static readonly int BrushUVID = Shader.PropertyToID("_BrushUV");
    private static readonly int BrushColorID = Shader.PropertyToID("_BrushColor");
    private static readonly int BrushOpacity = Shader.PropertyToID("_BrushOpacity");

    private void OnValidate()
    {
        _camera ??= Camera.main;
    }

    private void Update()
    {
        if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
            return;

        Vector2 uv = hit.textureCoord;
        Color actualColor = GetColorFromPreset(_colorPreset);

        // Set preview values on material
        _renderer.material.SetVector(BrushUVID, uv);
        _renderer.material.SetFloat(BrushRadiusID, _brushRadius);
        _renderer.material.SetColor(BrushColorID, actualColor);
        _renderer.material.SetFloat(BrushOpacity, _opacity);

        SetShaderColorPreset(_renderer.material, _colorPreset, "_COLORPRESET");

        if (Input.GetMouseButton(0))
        {
            _texturePainter.Paint(uv, actualColor, _brushRadius, _opacity);
        }
    }

    private void SetShaderColorPreset(Material mat, BrushColorPreset preset, string keywordName)
    {
        foreach (string presetName in System.Enum.GetNames(typeof(BrushColorPreset)))
        {
            mat.DisableKeyword($"{keywordName}_{presetName}");
        }

        mat.EnableKeyword($"{keywordName}_{preset}");
    }

    private static Color GetColorFromPreset(BrushColorPreset preset)
    {
        return preset switch
        {
            BrushColorPreset.RED => Color.red,
            BrushColorPreset.BLUE => Color.blue,
            BrushColorPreset.GREEN => Color.green,
            BrushColorPreset.YELLOW => Color.yellow,
            BrushColorPreset.PURPLE => new Color(0.5f, 0f, 1f),
            BrushColorPreset.PINK => new Color(1f, 0.4f, 0.8f),
            _ => Color.white
        };
    }

    private enum BrushColorPreset
    {
        RED,
        BLUE,
        GREEN,
        YELLOW,
        PURPLE,
        PINK
    }
}