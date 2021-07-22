using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionsController : MonoBehaviour
{
    [SerializeField] private List<ConnectionsController> connections = new List<ConnectionsController>();
    private List<LineController> connectedLines = new List<LineController>();
    [SerializeField] private GameObject line;
    private LineController currentLineDraw;
    private NodeController node;
    private void Start()
    {
        node = GetComponent<NodeController>();
    }
    public bool AddOutputConnection(ConnectionsController anotherNode)
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

                currentLineDraw.bindPoints = new List<Transform>();
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
