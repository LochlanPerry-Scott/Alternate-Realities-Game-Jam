using System;
using System.Collections.Generic;
using UnityEngine;

public class ShakeHandler : MonoBehaviour
{
    public static ShakeHandler Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Shake Detection")]
    public Action OnShake;
    [SerializeField] private float shakeDetectionThreashold = 2;
    [SerializeField] private float minShakeInterval;
    private float accelerometerUpdateInterval = 1 / 60;
    private float lowPassKernelWidthInSeconds = 1;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue;
    private float timeSinceLastShake;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreashold *= shakeDetectionThreashold;
        lowPassValue = Input.acceleration;
    }

    private void Update()
    {
        if (SystemInfo.supportsAccelerometer)
        {
            Vector3 acceleration = Input.acceleration;
            lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
            Vector3 deltaAcceleration = acceleration - lowPassValue;

            // Shake Detection
            if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreashold &&
                Time.unscaledTime >= timeSinceLastShake + minShakeInterval)
            {
                OnShake?.Invoke();
                timeSinceLastShake = Time.unscaledTime;
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    OnShake?.Invoke();
        //}
    }
}
