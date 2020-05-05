using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaypointCreator : MonoBehaviour
{
    public System.Action OnUpdateNode;
    public System.Action OnBeginNodeCreation;

    public GameObject nodePrefab;

    public Vector3 mousePositionWorld;

    public static List<NodeData> nodePositions = new List<NodeData>();

    private bool hasBeganPool = false;

    public bool isStartNode = false;
    public bool isEndNode = false;
    [Space]
    public bool worldContainsStartNode = false;
    public bool worldContainsEndNode = false;

    public void Initialize()
    {
        OnUpdateNode += SearchSurroundingNodes;
        OnBeginNodeCreation += BeginPool;
        //ObjectPooling.Init();
    }

    public void BeginPool()
    {
        Debug.Log("Generate Pool");
        //ObjectPooling.Init("Node", nodePrefab, 10 /* AstarPath.active.data.gridGraph.width * AstarPath.active.data.gridGraph.width */, transform);
        hasBeganPool = true;
    }

    public void RemoveNodes()
    {
        for (int i = 0; i < nodePositions.Count; i++)
        {
            nodePositions[i].nodeObject.ResetConnections();
            Pooling.TryPool(nodePositions[i].nodeObject.gameObject);
        }

        nodePositions.Clear();
    }

    public void RemoveNodeFromData(NodeData nodeToRemove)
    {
        if(nodeToRemove.isEnd)
        {
            worldContainsEndNode = false;
        }
        else if(nodeToRemove.isStart)
        {
            worldContainsStartNode = false;
        }

        Pooling.TryPool(nodeToRemove.nodeObject.gameObject);

        nodePositions.Remove(nodeToRemove);
    }

    public void SearchSurroundingNodes()
    {
        List<Waypoint> nodeWaypoints = NodeData2Waypoint(nodePositions);

        for (int i = 0; i < nodePositions.Count; i++)
        {
            Waypoint currentNode = nodePositions[i].nodeObject;

            currentNode.connectedTo = NodesToConnect(currentNode, nodePositions);
            currentNode.SetConnections(nodeWaypoints);
        }

        Debug.Log("Searching for all nodes");
    }

    List<Waypoint> NodeData2Waypoint(List<NodeData> nodeData)
    {
        List<Waypoint> nodeWaypoints = new List<Waypoint>();

        for (int i = 0; i < nodePositions.Count; i++)
        {
            nodeWaypoints.Add(nodeData[i].nodeObject);
        }

        return nodeWaypoints;
    }

    List<Waypoint> NodesToConnect(Waypoint currentNode, List<NodeData> nodes)
    {
        List<Waypoint> connectedNodes = new List<Waypoint>(nodePositions.Count);

        foreach (NodeData newNode in nodes)
        {
            float dist = Vector3.Distance(newNode.nodeObject.transform.position, currentNode.transform.position);
            if (dist <= 1.01f && dist >= .5f && newNode.nodeObject != currentNode)
            {
                if (newNode.nodeObject.connectedTo.Contains(currentNode))
                {
                    //Debug.LogWarning(newNode.name + " + " + currentNode.name + " are already connected (" + currentNode.name + ")");
                }
                else
                {
                    //Debug.Log("Setting Connections");
                    connectedNodes.Add(newNode.nodeObject);
                }
            }
        }

        return connectedNodes;
    }
}

public class NodeData
{
    public Int3 nodePosition;
    public Waypoint nodeObject;

    public bool isStart, isEnd = false;
}
