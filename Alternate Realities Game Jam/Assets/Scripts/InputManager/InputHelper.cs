using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Adds the input to <see cref="TouchCreator"/> to recognize the correct input
/// </summary>
public static class InputHelper
{
    // Resembles touching the screen but using mouse controls
    private static TouchCreator lastFakeTouch;

    public static List<Touch> GetTouches()
    {
        List<Touch> touches = new List<Touch>();
        touches.AddRange(Input.touches);

        // Uncomment if you want it only to allow mouse swipes in the Unity Editor
        //#if UNITY_EDITOR
        if (lastFakeTouch == null)
        {
            lastFakeTouch = new TouchCreator();
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastFakeTouch.Phase = TouchPhase.Began;
            lastFakeTouch.DeltaPosition = new Vector2(0, 0);
            lastFakeTouch.Position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            lastFakeTouch.FingerId = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastFakeTouch.Phase = TouchPhase.Ended;
            Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            lastFakeTouch.DeltaPosition = newPosition - lastFakeTouch.Position;
            lastFakeTouch.Position = newPosition;
            lastFakeTouch.FingerId = 0;
        }
        else if (Input.GetMouseButton(0))
        {
            lastFakeTouch.Phase = TouchPhase.Moved;
            Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            lastFakeTouch.DeltaPosition = newPosition - lastFakeTouch.Position;
            lastFakeTouch.Position = newPosition;
            lastFakeTouch.FingerId = 0;
        }
        else
        {
            lastFakeTouch = null;
        }

        if (lastFakeTouch != null)
        {
            touches.Add(lastFakeTouch.Create());
        }
        // Uncomment if you want it only to allow mouse swipes in the Unity Editor
        //#endif

        return touches;
    }
}