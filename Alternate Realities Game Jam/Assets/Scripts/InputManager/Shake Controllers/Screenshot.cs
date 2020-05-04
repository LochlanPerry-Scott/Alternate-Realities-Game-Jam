using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public static Screenshot instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private Camera mainCam;

    public Texture2D screenCapture;

    public static void TakeSnapshot(int width, int height, Camera _Camera, System.Action<Texture2D> callback)
    {
        instance._TakeSnapshot(width, height, _Camera, callback);
    }

    private void _TakeSnapshot(int _Width, int _Height, Camera _Camera, System.Action<Texture2D> callback)
    {
        mainCam = _Camera;

        mainCam.targetTexture = RenderTexture.GetTemporary(_Width, _Height, 16);

        StartCoroutine(_Screenshot(_Width, _Height, _Camera, callback));
    }

    private IEnumerator _Screenshot(int _Width, int _Height, Camera _Camera, System.Action<Texture2D> callback)
    {
        yield return new WaitForEndOfFrame();

        RenderTexture camRender = mainCam.targetTexture;

        Texture2D renderResults = new Texture2D(camRender.width, camRender.height, TextureFormat.ARGB32, false);
        renderResults.ReadPixels(new Rect(0, 0, camRender.width, camRender.height), 0, 0);
        renderResults.Apply();

        callback(renderResults);

        RenderTexture.ReleaseTemporary(camRender);
        mainCam.targetTexture = null;
    }
}
