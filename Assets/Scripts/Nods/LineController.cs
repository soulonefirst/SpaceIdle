using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer _lR;
    private readonly Transform[] _bindPoints = new Transform[2];

    private void Awake()
    {
        _lR = GetComponent<LineRenderer>();
    }
    public void BindPoints(Transform source,Transform target)
    {
        _bindPoints[0]  = source;
        _bindPoints[1] = target.transform;
        SetLinePoints();
    }

    private void FixedUpdate()
    {
        SetLinePoints();
    }
    private void SetLinePoints()
    {
        Vector3[] bPoints = { _bindPoints[0].position, _bindPoints[1].position };
        _lR.SetPositions(bPoints);
    }
}
