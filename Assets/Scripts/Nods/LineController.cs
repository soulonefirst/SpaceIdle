using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer _lR;
    private Transform[] _bindedPoints = new Transform[2];

    private void Start()
    {
        _lR = GetComponent<LineRenderer>();
        SetLinePoints();
    }
    public void BindPoints(Transform source,Transform target)
    {
        _bindedPoints[0]  = source;
        _bindedPoints[1] = target.transform;
    }

    private void FixedUpdate()
    {
        SetLinePoints();
    }
    private void SetLinePoints()
    {
        Vector3[] bPoints = { _bindedPoints[0].position, _bindedPoints[1].position };
        _lR.SetPositions(bPoints);
    }
}
