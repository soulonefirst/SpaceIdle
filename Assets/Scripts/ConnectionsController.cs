using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionsController : MonoBehaviour
{
    [SerializeField] private List<ConnectionsController> connections = new List<ConnectionsController>();
    [SerializeField] private GameObject line;
    [SerializeField] private LineController currentLineDraw;

    private List<LineController> connectedLines = new List<LineController>();
    private NodeController node;
    [SerializeField] private Transform circle;
    private void Start()
    {
        node = GetComponent<NodeController>();
        circle = transform.GetChild(1).transform;
    }
    public bool AddConnection(ConnectionsController anotherNode)
    {
        foreach(ConnectionsController cc in connections)
        {
            if (cc == anotherNode || this == anotherNode)
            {
                return false;
            }
        }
        foreach(string req in anotherNode.node.options.Requirements)
        {
            if (node.options.Product == req)
            {
                connections.Add(anotherNode);
                connectedLines.Add(currentLineDraw);
                anotherNode.connections.Add(this);
                anotherNode.connectedLines.Add(currentLineDraw);

                currentLineDraw.bindPoints.Add(node.transform);
                currentLineDraw.bindPoints.Add(anotherNode.transform);
                currentLineDraw.enabled = false;
                return true;
            }
        }
        return false;
    }

    public void StartDrawLine(InputsController inputsController)
    {
        var gO = Instantiate(line, transform);
        currentLineDraw = gO.GetComponent<LineController>();
        currentLineDraw.inputsCont = inputsController;
    }
    public void CancelDrawLine()
    {
        Destroy(currentLineDraw.gameObject);
    }
    public void ActivateConnectdLines(bool b)
    {
        if(connectedLines.Count > 0)
        {
            foreach(LineController lineController in connectedLines)
            {
                lineController.enabled = b;
            }
        }
    }
}
