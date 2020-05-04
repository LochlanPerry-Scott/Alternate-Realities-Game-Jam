using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Touch creator.
/// </summary>
public class TouchCreator
{
    static BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
    static Dictionary<string, FieldInfo> fields;

    private object Touch;

    public float DeltaTime
    {
        get
        {
            return ((Touch)Touch).deltaTime;
        }
        set
        {
            fields["m_TimeDelta"].SetValue(Touch, value);
        }
    }

    public int TapCount
    {
        get
        {
            return ((Touch)Touch).tapCount;
        }
        set
        {
            fields["m_TapCount"].SetValue(Touch, value);
        }
    }

    public TouchPhase Phase
    {
        get
        {
            return ((Touch)Touch).phase;
        }
        set
        {
            fields["m_Phase"].SetValue(Touch, value);
        }
    }

    public Vector2 DeltaPosition
    {
        get
        {
            return ((Touch)Touch).deltaPosition;
        }
        set
        {
            fields["m_PositionDelta"].SetValue(Touch, value);
        }
    }

    public int FingerId
    {
        get
        {
            return ((Touch)Touch).fingerId;
        }
        set
        {
            fields["m_FingerId"].SetValue(Touch, value);
        }
    }

    public Vector2 Position
    {
        get
        {
            return ((Touch)Touch).position;
        }
        set
        {
            fields["m_Position"].SetValue(Touch, value);
        }
    }

    public Vector2 RawPosition
    {
        get
        {
            return ((Touch)Touch).rawPosition;
        }
        set
        {
            fields["m_RawPosition"].SetValue(Touch, value);
        }
    }

    public Touch Create()
    {
        return (Touch)Touch;
    }

    public TouchCreator()
    {
        Touch = new Touch();
    }

    static TouchCreator()
    {
        fields = new Dictionary<string, FieldInfo>();
        foreach (FieldInfo fInfo in typeof(Touch).GetFields(Flags))
        {
            fields.Add(fInfo.Name, fInfo);
            //Debug.Log("name: " + f.Name); // Use this to find the names of hidden private fields
        }
    }
}