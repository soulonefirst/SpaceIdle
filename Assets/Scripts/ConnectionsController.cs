using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private List<Transform> outputConnections;
    [SerializeField]
    private List<Transform> inputConnections;
    public bool lineIsDraw = false;


    public ConnectionsController StartDrawLine()
    {
        GameObject line = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);      
        line.GetComponent<LineController>().points.Add(transform);
        return this;
    }
    public ConnectionsController AddConnection(GameObject target)
    {
        Debug.Log(target.name);
        return this;
    }
    public ConnectionsController StopDrawLine()
    {
        return this;
    }
}
