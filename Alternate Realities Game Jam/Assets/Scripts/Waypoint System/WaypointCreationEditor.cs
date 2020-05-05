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

    private void OnEnable()
    {
        thisObject = (WaypointCreator)target;

        // Make sure the AstarPath object has been loaded
        AstarPath.FindAstarPath();

        AstarPath.active.Scan();

        thisObject.Initialize();
    }

    void OnSceneGUI()
    {
        if (m_editMode)
        {
            Event e = Event.current;

            if (e.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
            }

            Handles.BeginGUI();
            if (!thisObject.isEndNode)
            {
                if (!thisObject.worldContainsStartNode)
                {
                    Rect rect = new Rect(10, 10, 150, 20);
                    GUI.Box(rect, "Create Start Node");
                    if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
                    {
                        //Selection.activeGameObject = TestScript.selected;
                        thisObject.isStartNode = !thisObject.isStartNode;
                        Debug.Log("Placing Start Node");
                    }
                }
            }

            if (!thisObject.isStartNode)
            {
                if (!thisObject.worldContainsEndNode)
                {
                    Rect rect = new Rect(10, 40, 150, 20);
                    GUI.Box(rect, "Create End Node");
                    if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
                    {
                        //Selection.activeGameObject = TestScript.selected;
                        thisObject.isEndNode = !thisObject.isEndNode;
                        Debug.Log("Placing End Node");
                    }
                }
            }

            Handles.EndGUI();

            if (Event.current.modifiers == EventModifiers.Alt)
            {
                //Debug.Log("Rotating Scene Camera");
                //onSwipe?.Invoke(Event.current); // Fire event
                viewKeyHeld = true;
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

                            Quaternion finalRot = Quaternion.identity;
                            if (thisObject.isStartNode || thisObject.isEndNode)
                                finalRot = Quaternion.identity;
                            else
                                finalRot = Quaternion.Euler(new Vector3(0, 45, 0));

                            //GameObject _nodeObject = ObjectPooling.SpawnFromPool("Node", nodePos, finalRot, thisObject.transform);
                            GameObject _nodeObject = Pooling.GetObject(thisObject.nodePrefab);
                            _nodeObject.name = "Node " + WaypointCreator.nodePositions.Count;
                            _nodeObject.transform.position = nodePos;
                            _nodeObject.transform.rotation = finalRot;

                            NodeData newNode = new NodeData
                            {
                                isStart = thisObject.isStartNode,
                                isEnd = thisObject.isEndNode,
                                nodePosition = node.position,
                                nodeObject = _nodeObject.GetComponent<Waypoint>()
                            };

                            WaypointCreator.nodePositions.Add(newNode);

                            if (thisObject.isStartNode)
                            {
                                _nodeObject.name = "Start Node";
                                thisObject.worldContainsStartNode = true;
                            }
                            else if (thisObject.isEndNode)
                            {
                                _nodeObject.name = "End Node";
                                thisObject.worldContainsEndNode = true;
                            }

                            thisObject.isStartNode = false;
                            thisObject.isEndNode = false;

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
                if(!thisObject.worldContainsEndNode || !thisObject.worldContainsStartNode)
                {
                    Debug.LogError("Make sure the path contains a valid Start and End node.");
                    return;
                }

                //AstarPath.FindAstarPath();

                AstarPath.active.Scan();

                thisObject.OnUpdateNode?.Invoke();
                m_editMode = false;
            }

            if (GUILayout.Button("Cancel", GUILayout.Width(60)))
            {
                thisObject.worldContainsEndNode = false;
                thisObject.worldContainsStartNode = false;
                thisObject.isStartNode = false;
                thisObject.isEndNode = false;

                thisObject.RemoveNodes();

                AstarPath.active.Scan();
                m_editMode = false;
            }
        }
        else
        {
            if (GUILayout.Button("Create Node Track", GUILayout.Width(200)))
            {
                thisObject.OnBeginNodeCreation?.Invoke();
                m_editMode = true;
            }

            if (GUILayout.Button("Remove All Nodes", GUILayout.Width(200)))
            {
                thisObject.worldContainsEndNode = false;
                thisObject.worldContainsStartNode = false;
                thisObject.isStartNode = false;
                thisObject.isEndNode = false;

                thisObject.RemoveNodes();

                AstarPath.active.Scan();
                m_editMode = false;
            }
        }
        GUILayout.EndHorizontal();
    }
}
