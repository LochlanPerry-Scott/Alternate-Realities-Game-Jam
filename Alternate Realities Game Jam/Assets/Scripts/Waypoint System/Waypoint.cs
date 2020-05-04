using UnityEngine;
using System.Collections;
using System;
using Pathfinding;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour
{
    public GraphUpdateScene nodeEditor;
    private Transform node;
    public Transform[] nodeConnectorPivots;
    public List<Waypoint> connectedTo = new List<Waypoint>();
    [Space]
    [HideInInspector] public Color nodeColor;

    public void SetConnections()
    {
        node = transform;

        for (int i = 0; i < nodeConnectorPivots.Length; i++)
        {
            Transform nodeConnection = nodeConnectorPivots[i];
            nodeConnection.gameObject.SetActive(false);
            SetColorData(nodeConnection);
        }

        for (int i = 0; i < connectedTo.Count; i++)
        {
            Transform nodeConnection = nodeConnectorPivots[i];
            SetColorData(nodeConnection);
            nodeConnection.gameObject.SetActive(true);
            nodeConnection.transform.LookAt(connectedTo[i].transform.position);
        }
    }

    public void SetColorData(Transform beginPoint)
    {
        SpriteRenderer newNode = node.transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer connector = beginPoint.GetChild(0).GetComponent<SpriteRenderer>();
        connector.color = nodeColor;
        newNode.color = nodeColor;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(transform.position, 1);
    //}
}