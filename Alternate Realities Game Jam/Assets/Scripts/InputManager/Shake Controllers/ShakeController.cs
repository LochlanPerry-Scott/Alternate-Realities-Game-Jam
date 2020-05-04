using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShakeController : MonoBehaviour
{
    public RectTransform snapshotHolder;
    public RawImage Reality1;
    public RawImage Reality2;

    public enum CurrentReality
    {
        Reality1,
        Reality2
    }
    public CurrentReality currentReality;

    private void Start()
    {
        ShakeHandler.Instance.OnShake += OnShakeDevice;
    }

    //private void OnDestroy()
    //{
    //    ShakeHandler.Instance.OnShake -= OnShakeDevice;
    //}

    private void OnShakeDevice()
    {
        Sequence s = DOTween.Sequence();

        s.AppendCallback(() => snapshotHolder.DOLocalRotate(new Vector3(0, (snapshotHolder.localEulerAngles.y == 0) ? 180 : 0, 0), .5f, RotateMode.Fast));
        s.AppendInterval(.15f);
        s.AppendCallback(() => OnChangeRealities());
        

        //Screenshot.TakeSnapshot(Screen.width, Screen.height, Camera.main, (status) =>
        //{
        //    if (status != null)
        //        SetSnapshot(status);
        //});
    }

    private void OnChangeRealities()
    {
        if (currentReality == CurrentReality.Reality1)
        {
            Reality1.DOColor(new Color(Reality1.color.r, Reality1.color.g, Reality1.color.b, 0), 0.01f);
            Reality2.DOColor(new Color(Reality2.color.r, Reality2.color.g, Reality2.color.b, 255), 0.01f);
            Debug.Log("1");
            currentReality = CurrentReality.Reality2;

            return;
        }
        else
        {
            Reality1.DOColor(new Color(Reality1.color.r, Reality1.color.g, Reality1.color.b, 255), 0.01f);
            Reality2.DOColor(new Color(Reality2.color.r, Reality2.color.g, Reality2.color.b, 0), 0.01f);
            Debug.Log("2");
            currentReality = CurrentReality.Reality1;

            return;
        }
    }

    private void SetSnapshot(Texture2D snapshot)
    {
        //snapshotHolder.texture = snapshot;
        //snapshotHolder.CrossFadeAlpha(255, 0, true);
    }
}
