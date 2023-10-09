using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bloom : MonoBehaviour
{
    public Material bloomMaterial;
    [Range(0, 10)]
    public float bloomIntensity = 1.0f;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(bloomMaterial == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        bloomMaterial.SetFloat("_Intensity", bloomIntensity);

        Graphics.Blit(source, destination, bloomMaterial);
    }
}
