using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public SwipeManager swipeManager;
    private bool heldDown = false;
    private SwipeAction currentAction;

    private void Awake()
    {
        swipeManager.onLongPress += HoldRotation;
    }

    private void HoldRotation(SwipeAction swipeAction)
    {
        currentAction = swipeAction;
        heldDown = true;
    }

    private void Update()
    {

    }
}
