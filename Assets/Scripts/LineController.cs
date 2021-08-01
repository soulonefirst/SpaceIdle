using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lR;
    public InputsController inputsCont;
    public List<Transform> bindPoints = new List<Transform>();

    private void Start()
    {
        lR = GetComponent<LineRenderer>();
    }


    private void FixedUpdate()
    {
        if (bindPoints.Count==0)
        {
            Vector3[] points = {transform.position,inputsCont.MousePosition()};
            lR.SetPositions(points);
        }
        else
        {
            Vector3[] bPoints = {bindPoints[0].position, bindPoints[1].position };
            lR.SetPositions(bPoints);
        }
    }
}
