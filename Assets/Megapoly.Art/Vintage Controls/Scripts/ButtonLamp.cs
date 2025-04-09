using UnityEngine;
using System.Collections;

public class ButtonLamp : MonoBehaviour
{
    public enum eColor
    {
        Red,
        Yellow,
        Green,
        Blue,
    }

    public bool on;
    public Transform lamp;
    public eColor lightColor;
    public Light light;

    [Header("Light Intensity Settings")]
    public float intensityOn = 1.0f;
    public float intensityOff = 0.0f;
    public float fadeDuration = 0.5f;

    Renderer rend;
    private Coroutine _fadeCoroutine;

    void Awake()
    {
        rend = lamp.GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(on, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            switch (lightColor)
            {
                case eColor.Red:
                    rend.material.SetColor("_EmissionColor", new Color(1f, 0f, 0.02f, 1f));
                    break;
                case eColor.Yellow:
                    rend.material.SetColor("_EmissionColor", new Color(1f, 0.65f, 0f, 1f));
                    break;
                case eColor.Green:
                    rend.material.SetColor("_EmissionColor", new Color(0.15f, 1f, 0f, 1f));
                    break;
                case eColor.Blue:
                    rend.material.SetColor("_EmissionColor", new Color(0f, 0.33f, 1f, 1f));
                    break;
                default:
                    break;
            }
        }
        else
        {
            rend.material.SetColor("_EmissionColor", new Color(0.0f, 0.0f, 0.0f, 0.0f));
        }
    }

    public void SetState(bool state, bool immediate = false)
    {
     
        on = state;
        Color targetEmissionColor;

        switch (lightColor)
        {
            case eColor.Red:
                targetEmissionColor = on ? new Color(1f, 0f, 0.02f, 1f) : new Color(0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case eColor.Yellow:
                targetEmissionColor = on ? new Color(1f, 0.65f, 0f, 1f) : new Color(0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case eColor.Green:
                targetEmissionColor = on ? new Color(0.15f, 1f, 0f, 1f) : new Color(0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case eColor.Blue:
                targetEmissionColor = on ? new Color(0f, 0.33f, 1f, 1f) : new Color(0.0f, 0.0f, 0.0f, 0.0f);
                break;
            default:
                targetEmissionColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                break;
        }

        rend.material.SetColor("_EmissionColor", targetEmissionColor);

        if (light != null)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            if (immediate || fadeDuration <= 0)
            {
                light.intensity = on ? intensityOn : intensityOff;
            }
            else
            {
                _fadeCoroutine = StartCoroutine(FadeLightIntensity(on));
            }
        }
    }

    private IEnumerator FadeLightIntensity(bool turnOn)
    {
        float targetIntensity = turnOn ? intensityOn : intensityOff;
        float startIntensity = light.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / fadeDuration);
            light.intensity = currentIntensity;
            yield return null;
        }

        light.intensity = targetIntensity;
        _fadeCoroutine = null;
    }
}