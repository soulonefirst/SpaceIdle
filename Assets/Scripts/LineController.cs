using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public ConnectionsController connectionsController;
    private LineRenderer lR;
    private Transform[] bindPoints = new Transform[2];

    private void Start()
    {
        lR = GetComponent<LineRenderer>();
        SetLinePoints();
    }
    public void BindPoints(Transform target)
    {
        bindPoints[0]  = transform;
        bindPoints[1] = target;
    }

    private void FixedUpdate()
    {
        SetLinePoints();
    }
    private void SetLinePoints()
    {
        Vector3[] bPoints = {bindPoints[0].position, bindPoints[1].position };
        lR.SetPositions(bPoints);
    }
}
