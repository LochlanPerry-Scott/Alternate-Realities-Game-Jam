#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using Pathfinding;
using System.Collections.Generic;
using DG.Tweening;

[CustomEditor(typeof(WaypointContainer))]
public class WaypointContainerEditor : Editor
{
    private static bool m_editMode = false;
    private static int m_count = 0;

    public Color nodeColor = new Color(154/255, 154/255, 154/255, 136/255);
    public WaypointContainer selectedObject;

    private bool openedList = false;
    private bool hasCompletedList = false;
    private bool hasScannedBefore = false;


    //protected virtual void OnSceneGUI()
    //{
    //    if (Event.current.type == EventType.Repaint)
    //    {
    //        Transform transform = ((WaypointContainer)target).transform;
    //    //    Handles.color = Handles.xAxisColor;
    //    //    Handles.CircleHandleCap(
    //    //        0,
    //    //        transform.position + new Vector3(3f, 0f, 0f),
    //    //        transform.rotation * Quaternion.LookRotation(Vector3.right),
    //    //        size,
    //    //        EventType.Repaint
    //    //    );
    //    }
    //}

    public override void OnInspectorGUI()
    {
        if(selectedObject == null)
        {
            selectedObject = (WaypointContainer)target;
        }

        if(m_editMode)
        {
            //serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("nodeList"));
            //openedList = EditorGUI.DropdownButton(new Rect(10, 0, 100, 50), GUIContent.none, FocusType.Passive);
            serializedObject.ApplyModifiedProperties();
        }

        nodeColor = EditorGUILayout.ColorField("Node Color", nodeColor);

        if (GUILayout.Button("Generate Pathfinding"))
        {
            if (selectedObject.nodeList.Count == selectedObject.transform.childCount)
            {
                Debug.Log("Update");

                selectedObject.nodeList.Clear();
            }

            m_editMode = true;
            hasCompletedList = false;

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("nodeList"));
            serializedObject.ApplyModifiedProperties();

            for (int i = 0; i < selectedObject.transform.childCount; i++)
            {
                Waypoint selectedChild = selectedObject.transform.GetChild(i).gameObject.GetComponent<Waypoint>();

                if (selectedObject.nodeList.Contains(selectedChild))
                {
                    Debug.Log("Need To Replace");
                }
                else
                {
                    selectedChild.nodeColor = nodeColor;
                    selectedObject.nodeList.Add(selectedChild);
                }
            }

            Debug.Log("Calling");
            UpdateConnectors();

            if (!hasScannedBefore)
            {
                // Recalculate all graphs
                AstarPath.active.Scan();
                hasScannedBefore = true;
            }
        }

        if (GUILayout.Button("Reset Node Data"))
        {
            m_editMode = false;
            hasCompletedList = false;

            for (int i = 0; i < selectedObject.nodeList.Count; i++)
            {
                Waypoint selectedNode = null;

                selectedNode = selectedObject.nodeList[i];
                selectedNode.connectedTo.Clear();                
                //selectedNode.SetConnections();
            }

            selectedObject.nodeList.Clear();

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("nodeList"));
            serializedObject.ApplyModifiedProperties();

            hasScannedBefore = false;
        }
    }

    List<Waypoint> NodesToConnect(Waypoint currentNode, List<Waypoint> nodes)
    {
        List<Waypoint> connectedNodes = new List<Waypoint>(4);

        foreach (Waypoint newNode in nodes)
        {
            float dist = Vector3.Distance(newNode.transform.position, currentNode.transform.position);
            if (dist <= 1f && dist >= .5f && newNode != currentNode)
            {
                if (newNode.connectedTo.Contains(currentNode))
                {
                    //Debug.LogWarning(newNode.name + " + " + currentNode.name + " are already connected (" + currentNode.name + ")");
                }
                else
                {
                    //Debug.Log("Setting Connections");
                    connectedNodes.Add(newNode);
                }
            }
        }

        return connectedNodes;
    }

    private void UpdateConnectors()
    {
        for (int i = 0; i < selectedObject.nodeList.Count; i++)
        {
            Waypoint currentNode = selectedObject.nodeList[i];

            currentNode.connectedTo = NodesToConnect(currentNode, selectedObject.nodeList);
            //currentNode.SetConnections();
        }
    }
}
#endif
