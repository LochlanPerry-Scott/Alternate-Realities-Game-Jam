using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pathfinding;

[CustomEditor(typeof(WaypointCreator))]
public class WaypointCreationEditor : Editor
{
    WaypointCreator thisObject;
    public bool m_editMode = false;
    private bool viewKeyHeld = false;
    private int waypointNumber = 0;

    private void OnEnable()
    {
        thisObject = (WaypointCreator)target;

        // Make sure the AstarPath object has been loaded
        AstarPath.FindAstarPath();

        AstarPath.active.Scan();

        thisObject.Initialize();

        thisObject.BeginPool();
    }

    bool once = false;
    bool sndOnce = false;

    void OnSceneGUI()
    {
        if (m_editMode)
        {
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
            }

            if (Event.current.modifiers == EventModifiers.Alt)
            {
                //Debug.Log("Rotating Scene Camera");
                //onSwipe?.Invoke(Event.current); // Fire event
                viewKeyHeld = true;
                once = true;
            }
            else
            {
                //Debug.Log("No Longer Rotating Scene Camera");

                if (Event.current.type == EventType.MouseDown)
                {
                    //Debug.LogWarning("Testing For Call");
                    Camera cam = SceneView.lastActiveSceneView.camera;

                    Vector3 mousepos = Event.current.mousePosition;
                    mousepos.z = -cam.worldToCameraMatrix.MultiplyPoint(thisObject.transform.position).z;
                    mousepos.y = Screen.height - mousepos.y - 36.0f; // ??? Why that offset?!
                    mousepos = cam.ScreenToWorldPoint(mousepos);

                    thisObject.mousePositionWorld = mousepos;

                    RaycastHit hit;
                    if (Physics.Raycast(cam.transform.position, thisObject.mousePositionWorld - cam.transform.position, out hit, 200))
                    {
                        if (hit.transform != null)
                        {
                            // Check if the first grid graph in the scene has any nodes
                            // if it doesn't then it is not scanned.
                            if (AstarPath.active.data.gridGraph.nodes == null)
                                AstarPath.active.Scan();

                            NNInfo info = AstarPath.active.GetNearest(hit.point);
                            GraphNode node = info.node;

                            for (int i = 0; i < WaypointCreator.nodePositions.Count; i++)
                            {
                                if (node.position == WaypointCreator.nodePositions[i].nodePosition)
                                {
                                    Debug.Log("Removing Node");
                                    thisObject.RemoveNodeFromData(WaypointCreator.nodePositions[i]);
                                    return;
                                }
                            }

                            //thisObject.RemoveNodes();

                            Vector3 nodePos = (Vector3)node.position;

                            GameObject _nodeObject = ObjectPooling.SpawnFromPool("Node", nodePos, Quaternion.identity, thisObject.transform);
                            _nodeObject.name = "Waypoint " + waypointNumber;

                            NodeData newNode = new NodeData
                            {
                                nodePosition = node.position,
                                nodeObject = _nodeObject.GetComponent<Waypoint>()
                            };

                            WaypointCreator.nodePositions.Add(newNode);
                            //Debug.DrawLine(cam.transform.position, (Vector3)newNode.nodePosition, Color.yellow, 2);
                        }
                    }
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        //GUILayout.Toggle(viewKeyHeld, "is ALT held?", GUILayout.Width(50));
        DrawDefaultInspector();
        GUILayout.BeginHorizontal("box");
        if (m_editMode)
        {
            if (GUILayout.Button("Apply", GUILayout.Width(60)))
            {
                thisObject.OnUpdateNode?.Invoke();
                m_editMode = false;
            }
        }
        else
        {
            if (GUILayout.Button("Create Node Track", GUILayout.Width(200)))
            {
                m_editMode = true;
            }

            if (GUILayout.Button("Remove All Nodes", GUILayout.Width(200)))
            {
                Debug.LogError(WaypointCreator.nodePositions.Count);

                thisObject.RemoveNodes();


                WaypointCreator.nodePositions.Clear();
                m_editMode = false;
            }
        }
        GUILayout.EndHorizontal();
    }
}
