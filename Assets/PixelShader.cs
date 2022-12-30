using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelShader")]
public class PixelShader : MonoBehaviour
{
    public int width = 720;
    private int height;

    public Camera cam;

    protected void Start()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {

        float ratio = ((float)cam.pixelHeight / (float)cam.pixelWidth);
        height = Mathf.RoundToInt(width * ratio);

    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(width, height, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}