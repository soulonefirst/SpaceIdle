using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionsController : MonoBehaviour
{
    [SerializeField] private List<ConnectionsController> connections = new List<ConnectionsController>();
    [SerializeField] private GameObject line;
    [SerializeField] private List<LineController> currentLinesDraw = new List<LineController>();

    [SerializeField]private List<LineController> connectedLines = new List<LineController>();
    private NodeController node;
    private GameObject circle;
    private void Start()
    {
        node = GetComponent<NodeController>();
        circle = transform.GetChild(1).gameObject;
    }
    public void SetConnectionArea(bool dragablle)
    {
        if (!dragablle)
        {
            InputsController.startDrag += DrawCircle;
        }
    }
    public bool AddConnection(ConnectionsController anotherNode)
    {
        foreach (ConnectionsController cc in connections)
        {
            if (cc == anotherNode || this == anotherNode)
            {
                return false;
            }
        }
        foreach (string req in anotherNode.node.options.Requirements)
        {
            if (node.options.Product == req)
            {
                connections.Add(anotherNode);
                connectedLines.AddRange(currentLinesDraw);

                currentLinesDraw.Clear();
                return true;
            }
        }
        return false;
    }

    public void StartDrawLine(ConnectionsController lineTarget)
    {
        if (!connections.Contains(lineTarget))
        {
            var gO = Instantiate(line, transform);
            var LC = gO.GetComponent<LineController>();
            LC.BindPoints(lineTarget.transform);
            LC.connectionsController = lineTarget;
            currentLinesDraw.Add(LC);
        }
    }
    public void CancelDrawLine(ConnectionsController connect)
    {
        if (currentLinesDraw.Count > 0)
        {
            for (int i = 0; i < currentLinesDraw.Count; i++)
            {
                if (currentLinesDraw[i].connectionsController.Equals(connect))
                {
                    Destroy(currentLinesDraw[i].gameObject);
                    currentLinesDraw.Remove(currentLinesDraw[i]);
                    break;
                }
            }
        }
    }
    public void ActivateConnectedLines(bool b)
    {
        if (connectedLines.Count > 0)
        {
            foreach (LineController lineController in connectedLines)
            {
                lineController.enabled = b;
            }
        }
    }
    public void DeleteConnection(List<ConnectionsController> hitObjects)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            if (!hitObjects.Contains(connections[i]))
            {
                Destroy(connectedLines[i].gameObject);
                connectedLines.Remove(connectedLines[i]);
                connections.Remove(connections[i]);
            }
        }
    }
    private void DrawCircle(bool b)
    {
        circle.SetActive(b);
    }
}
