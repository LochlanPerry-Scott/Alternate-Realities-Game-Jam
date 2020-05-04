using UnityEngine;
using System.Collections.Generic;

public struct SwipeAction
{
    public SwipeDirection direction;
    public Vector2 rawDirection;
    public Vector2 clampedDirection;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float startTime;
    public float endTime;
    public float duration;
    public bool longPress;
    public bool shortPress;
    public float distance;
    public float longestDistance;
}

public enum SwipeDirection
{
    None,   // Basically means an invalid swipe
    Hold, 
    Tap,
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}

/// <summary>
/// Manager that converts input into useable functions
/// </summary>
public class SwipeManager : MonoBehaviour
{
    public static SwipeManager Instance
    {
        get;
        private set;
    }

    public System.Action<SwipeAction> onSwipe;
    public System.Action<SwipeAction> onLongPress;
    public System.Action<SwipeAction> onShortPress;

    [Range(0f, 200f)]
    public float minSwipeLength = 100f;

    public float holdDuration = 0.15f;
    //public float longHoldDuration = 0.6f;

    [Space]
    public TMPro.TextMeshProUGUI DebugText;
    public bool useDebug = false;

    private Vector2 currentSwipe;
    private SwipeAction currentSwipeAction = new SwipeAction();

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

        onSwipe += SwipeDetection;
        onLongPress += SwipeDetection;
    }

    private void Update()
    {
        DetectSwipe();
    }

    public void DetectSwipe()
    {
        List<Touch> touches = InputHelper.GetTouches();
        if (touches.Count > 0)
        {
            Touch touch = touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                ResetCurrentSwipeAction(touch);
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                UpdateCurrentSwipeAction(touch);
                if (!currentSwipeAction.longPress && currentSwipeAction.duration > holdDuration && currentSwipeAction.longestDistance < minSwipeLength)
                {
                    currentSwipeAction.direction = SwipeDirection.Hold; // Invalidate current swipe action
                    currentSwipeAction.longPress = true;
                    onLongPress?.Invoke(currentSwipeAction); // Fire event
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                UpdateCurrentSwipeAction(touch);

                if (!currentSwipeAction.shortPress && currentSwipeAction.duration <= holdDuration && currentSwipeAction.longestDistance < minSwipeLength)
                {
                    currentSwipeAction.direction = SwipeDirection.Tap; // Invalidate current swipe action
                    currentSwipeAction.shortPress = true;
                    onShortPress?.Invoke(currentSwipeAction); // Fire event
                }

                //// Make sure it was a legit swipe, not a tap, or long press
                //if (currentSwipeAction.distance < minSwipeLength || currentSwipeAction.longPress || currentSwipeAction.shortPress) // Didnt swipe enough or this is a long press
                //{
                //    currentSwipeAction.direction = (currentSwipeAction.longPress) ? SwipeDirection.Hold : SwipeDirection.Tap; // Invalidate current swipe action
                //}

                if (currentSwipeAction.distance > minSwipeLength)
                    onSwipe?.Invoke(currentSwipeAction); // Fire event
            }
        }
    }

    private void ResetCurrentSwipeAction(Touch touch)
    {
        currentSwipeAction.duration = 0f;
        currentSwipeAction.distance = 0f;
        currentSwipeAction.longestDistance = 0f;
        currentSwipeAction.longPress = false;
        currentSwipeAction.shortPress = false;
        currentSwipeAction.startPosition = new Vector2(touch.position.x, touch.position.y);
        currentSwipeAction.startTime = Time.time;
        currentSwipeAction.endPosition = currentSwipeAction.startPosition;
        currentSwipeAction.endTime = currentSwipeAction.startTime;
    }

    private void UpdateCurrentSwipeAction(Touch touch)
    {
        currentSwipeAction.endPosition = new Vector2(touch.position.x, touch.position.y);
        currentSwipeAction.endTime = Time.time;
        currentSwipeAction.duration = currentSwipeAction.endTime - currentSwipeAction.startTime;
        currentSwipe = currentSwipeAction.endPosition - currentSwipeAction.startPosition;
        currentSwipeAction.rawDirection = currentSwipe;
        currentSwipeAction.direction = GetSwipeDirection(currentSwipe);

        if(touch.phase == TouchPhase.Ended)
            currentSwipeAction.clampedDirection = ClampedSwipeDirection(currentSwipe);

        currentSwipeAction.distance = Vector2.Distance(currentSwipeAction.startPosition, currentSwipeAction.endPosition);
        if (currentSwipeAction.distance > currentSwipeAction.longestDistance) // If new distance is longer than previously longest
        {
            currentSwipeAction.longestDistance = currentSwipeAction.distance; // Update longest distance
        }
    }

    //public Vector3 ClampSwipeDistance()
    //{

    //}

    private Vector2 ClampedSwipeDirection(Vector2 direction)
    {
        float angle = Vector2.Angle(Vector2.up, direction.normalized); // Degrees
        Vector2 newClampDirection = Vector2.up;

        if (direction.x > 0) // Right
        {
            if (angle < 22.5f) // 0.0 - 22.5
            {
                // Up
                newClampDirection = Vector2.up;
            }
            else if (angle < 67.5f) // 22.5 - 67.5
            {
                // Up Right
                newClampDirection = new Vector2(1, 1);
            }
            else if (angle < 112.5f) // 67.5 - 112.5
            {
                // Right
                newClampDirection = Vector2.right;
            }
            else if (angle < 157.5f) // 112.5 - 157.5
            {
                // Down Right
                newClampDirection = new Vector2(1, -1);
            }
            else if (angle < 180.0f) // 157.5 - 180.0
            {
                // Down
                newClampDirection = Vector2.down;
            }
        }
        else // Left
        {
            if (angle < 22.5f) // 0.0 - 22.5
            {
                // Up
                newClampDirection = Vector2.up;
            }
            else if (angle < 67.5f) // 22.5 - 67.5
            {
                // Up Left
                newClampDirection = new Vector3(-1, 1);
            }
            else if (angle < 112.5f) // 67.5 - 112.5
            {
                // Left
                newClampDirection = Vector2.left;
            }
            else if (angle < 157.5f) // 112.5 - 157.5
            {
                // DownLeft
                newClampDirection = new Vector2(-1, -1);
            }
            else if (angle < 180.0f) // 157.5 - 180.0
            {
                // Down
                newClampDirection = Vector2.down;
            }
        }

        //Debug.DrawLine(new Vector3(10, 10, 10), newClampDirection * newClampDirection.magnitude, Color.green, 5, false);
        return newClampDirection.normalized;
    }

    private SwipeDirection GetSwipeDirection(Vector2 direction)
    {
        float angle = Vector2.Angle(Vector2.up, direction.normalized); // Degrees
        SwipeDirection swipeDirection = SwipeDirection.None;

        if (direction.x > 0) // Right
        {
            if (angle < 22.5f) // 0.0 - 22.5
            {
                swipeDirection = SwipeDirection.Up;
            }
            else if (angle < 67.5f) // 22.5 - 67.5
            {
                swipeDirection = SwipeDirection.UpRight;
            }
            else if (angle < 112.5f) // 67.5 - 112.5
            {
                swipeDirection = SwipeDirection.Right;
            }
            else if (angle < 157.5f) // 112.5 - 157.5
            {
                swipeDirection = SwipeDirection.DownRight;
            }
            else if (angle < 180.0f) // 157.5 - 180.0
            {
                swipeDirection = SwipeDirection.Down;
            }
        }
        else // Left
        {
            if (angle < 22.5f) // 0.0 - 22.5
            {
                swipeDirection = SwipeDirection.Up;
            }
            else if (angle < 67.5f) // 22.5 - 67.5
            {
                swipeDirection = SwipeDirection.UpLeft;
            }
            else if (angle < 112.5f) // 67.5 - 112.5
            {
                swipeDirection = SwipeDirection.Left;
            }
            else if (angle < 157.5f) // 112.5 - 157.5
            {
                swipeDirection = SwipeDirection.DownLeft;
            }
            else if (angle < 180.0f) // 157.5 - 180.0
            {
                swipeDirection = SwipeDirection.Down;
            }
        }

        return swipeDirection;
    }

    private void SwipeDetection(SwipeAction swipeAction)
    {
        if (useDebug)
        {
            switch (swipeAction.direction)
            {
                case SwipeDirection.None:
                    DebugText.text = " ";
                    break;

                case SwipeDirection.Hold:
                    DebugText.text = "Hold";
                    break;

                case SwipeDirection.Tap:
                    DebugText.text = "Tap";
                    break;

                case SwipeDirection.Up:
                    DebugText.text = "Swipe Up";
                    break;

                case SwipeDirection.UpRight:
                    DebugText.text = "Swipe Up Right";
                    break;

                case SwipeDirection.Right:
                    DebugText.text = "Swipe Right";
                    break;

                case SwipeDirection.DownRight:
                    DebugText.text = "Swipe Down Right";
                    break;

                case SwipeDirection.Down:
                    DebugText.text = "Swipe Down";
                    break;

                case SwipeDirection.DownLeft:
                    DebugText.text = "Swipe Down Left";
                    break;

                case SwipeDirection.Left:
                    DebugText.text = "Swipe Left";
                    break;

                case SwipeDirection.UpLeft:
                    DebugText.text = "Swipe Up Left";
                    break;
            }
        }
    }
}