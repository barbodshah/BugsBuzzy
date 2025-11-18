using UnityEngine;

public class MinigunHeatVisual : MonoBehaviour
{
    [Header("References")]
    public Minigun minigun;
    public Renderer targetRenderer;
    public Color coolColor = Color.black;
    public Color hotColor = Color.red;
    public float emissionIntensity = 2f;

    private Material heatMaterial;

    void Start()
    {
        if (targetRenderer != null)
        {
            heatMaterial = targetRenderer.material;
            heatMaterial.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        if (minigun == null || heatMaterial == null) return;

        float temp = Mathf.Clamp01(minigun.weaponTemp);
        Color finalColor = Color.Lerp(coolColor, hotColor, temp);

        heatMaterial.SetColor("_EmissionColor", finalColor * emissionIntensity);
    }
}
