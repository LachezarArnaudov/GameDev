using UnityEngine;

public class CameraShaderEffect : MonoBehaviour
{
    public Material effectMaterial;
    public bool isLowHealth = false;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isLowHealth && effectMaterial != null)
        {
            Graphics.Blit(source, destination, effectMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
