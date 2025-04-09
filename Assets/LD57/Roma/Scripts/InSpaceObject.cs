using LD57.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class InSpaceObject : MonoBehaviour
{
    public Objects ObjectType;
    public SpriteRenderer Sprite;
    public float DistanceFromViewer;
    public bool Exploreded;
    public string Name;
    public float _defaultScale;
    public float _maxScale;
    public float ResearchProgress;
    [Range(0f,1f)]
    public float TargetFocus;
    public float TargetZoom;
    public Color GlowColor;
    private Material _spriteMaterial;
    public bool Reserched;
    
    
    private readonly int _pixelSizeProperty = Shader.PropertyToID("_PixelateSize");
    private readonly int _glowColorProperty = Shader.PropertyToID("_GlowColor");
    private readonly int _glowIntensityProperty = Shader.PropertyToID("_Glow");
    private readonly int _glowGlobalIntensityProperty = Shader.PropertyToID("_GlowGlobal");

    private readonly int _blurProperty = Shader.PropertyToID("_BlurIntensity");
    private readonly int _waveStrengthProperty = Shader.PropertyToID("_RoundWaveStrength");
    private readonly int _waveSpeedProperty = Shader.PropertyToID("_RoundWaveSpeed");
    
    private void Start()
    {
        _defaultScale = Sprite.transform.localScale.x;
        transform.LookAt(Camera.main.transform);
        _spriteMaterial = Sprite.material;
        Sprite.material = _spriteMaterial;
        SetResearchedState(Reserched);
    }

    public void SetFocus(float focus)
    {
        if(Reserched) return;
        var blur =
            focus <= TargetFocus ? 
                Rom.MathHelper.Map(focus, 0f, TargetFocus, 77f, 0f) :
                Rom.MathHelper.Map(focus, TargetFocus, 1f, 0f, 77f);
        _spriteMaterial.SetFloat(_blurProperty, blur);

        var waveStrength = focus <= TargetFocus ? 
            Rom.MathHelper.Map(focus, 0f, TargetFocus, 0.126f, 0f):
            Rom.MathHelper.Map(focus, TargetFocus, 1f, 0, 0.166f);
        var waveSpeed = focus <= TargetFocus ? 
            Rom.MathHelper.Map(focus, 0f, TargetFocus, 0.2f, 0f):
            Rom.MathHelper.Map(focus, TargetFocus, 1f, 0f, 0.4f);
        _spriteMaterial.SetFloat(_waveStrengthProperty, waveStrength);
        _spriteMaterial.SetFloat(_waveSpeedProperty, waveSpeed);
    }
    
    public void SetZoom(float zoom)
    {
        if(Reserched) return;
        var scale =
            Sprite.transform.localScale.x > TargetZoom ? 
                (1f - ( Sprite.transform.localScale.x  - TargetZoom) /TargetZoom):
                Sprite.transform.localScale.x  / TargetZoom;
        var pixelSize =
            Rom.MathHelper.Map(scale, 0, 1f, -1000f, 512f);
        pixelSize = Mathf.Clamp(pixelSize, 20, 512);
        _spriteMaterial.SetFloat(_pixelSizeProperty, pixelSize);
    }
    
    public void SetResearchedState(bool state)
    {
        Reserched = state;
        
        _spriteMaterial.SetColor(_glowColorProperty,GlowColor);
        _spriteMaterial.SetFloat(_glowIntensityProperty,state ? 12f : 0f);
        _spriteMaterial.SetFloat(_glowGlobalIntensityProperty,state ? 12f :1f);
    }
}