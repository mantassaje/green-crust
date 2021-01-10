using System;
using UnityEngine;
public class TextMeshSharpener : MonoBehaviour
{
    /*
    Makes TextMesh look sharp regardless of camera size/resolution
    Do NOT change character size or font size; use scale only
    */
    // Properties
    private float lastPixelHeight = -1;
    private TextMesh textMesh;
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        resize();
    }
    void Update()
    {
        // Always resize in the editor, or when playing the game, only when the resolution changes
        if (Singles.CameraControlls.Camera.pixelHeight != lastPixelHeight || (Application.isEditor && !Application.isPlaying)) resize();
    }
    private void resize()
    {
        float ph = Singles.CameraControlls.Camera.pixelHeight;
        float ch = Singles.CameraControlls.Camera.orthographicSize;
        float pixelRatio = (ch * 2.0f) / ph;
        float targetRes = 1f;
        textMesh.characterSize = pixelRatio * Singles.CameraControlls.Camera.orthographicSize / Math.Max(transform.localScale.x, transform.localScale.y);
        textMesh.fontSize = (int)Math.Round(targetRes / textMesh.characterSize);
        lastPixelHeight = ph;
    }
}