using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour
{
    public Material mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //source is normal screen to send to monitor. We are intercepting this
        Graphics.Blit(source, destination, mat);
    }
}
