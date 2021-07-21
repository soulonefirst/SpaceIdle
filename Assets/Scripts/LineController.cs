using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lR;
    public List<Transform> points;
    private Transform pointer;


    private void Start()
    {
        lR = GetComponent<LineRenderer>();
        pointer = new GameObject("Pointer").transform;
        points.Add(pointer);
    }


    private void FixedUpdate()
    {
        pointer.position = GetWorldCoordinate(Input.mousePosition);
        for(int i = 0; i < points.Count; i++)
        {
            lR.SetPosition(i, points[i].position);
        }
    }
    private Vector3 GetWorldCoordinate(Vector3 mousePosition)
    {
        Vector3 mousePoint = new Vector3(mousePosition.x, mousePosition.y, 10);
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
