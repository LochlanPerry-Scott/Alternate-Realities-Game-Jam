using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public UnityEngine.UI.RawImage snapshotHolder;

    public float shakeDetectionThreashold;
    public float minShakeInterval;

    private float sqrShakeDetectionThreashold;
    private float timeSinceLastShake;

    private void Start()
    {
        sqrShakeDetectionThreashold = Mathf.Pow(shakeDetectionThreashold, 2);

    }

    private void LateUpdate()
    {
        if(Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreashold && Time.unscaledTime >= timeSinceLastShake + minShakeInterval)
        {
            
            if (Screenshot.instance.screenCapture != null)
                snapshotHolder.texture = Screenshot.instance.screenCapture;
            timeSinceLastShake = Time.unscaledTime;
        }
    }
}
