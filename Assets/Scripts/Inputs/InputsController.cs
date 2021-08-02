
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputsController : MonoBehaviour
{
    private PlayerInputs playerInput;
    private Camera cam;
    [SerializeField] private List<ConnectionsController> connectionTargets = new List<ConnectionsController>();
    [SerializeField] private ConnectionsController dragObject;
    private int draggableMask;
    private int connectingMask;

    public static event Action<bool> startDrag;
    private void Start()
    {
        cam = Camera.main;
        draggableMask = LayerMask.GetMask("Draggable");
        connectingMask = LayerMask.GetMask("Connecting");
    }
    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInputs();
        }
        playerInput.Main.Enable();

        playerInput.Main.LeftClick.started += x => LeftClickStart(GetRaycastHit(draggableMask));
        playerInput.Main.LeftClick.canceled += x => LeftClickEnd(GetRaycastHit(connectingMask));
        playerInput.Main.MousePosition.performed += x => MouseMove();
    }

    private void OnDisable()
    {
        playerInput.Main.Disable();
    }
    private void LeftClickStart(RaycastHit2D hit)
    {
        if (hit && hit.transform.TryGetComponent(out ConnectionsController connect))
        {
            dragObject = connect;
            dragObject.ActivateConnectedLines(true);
            startDrag?.Invoke(true);
        }
    }
    private void LeftClickEnd(RaycastHit2D hit)
    {
        if (dragObject != null && connectionTargets.Count > 0)
        {
            foreach (ConnectionsController connection in connectionTargets)
            {
                dragObject.AddConnection(connection);
            }
        }
        if (dragObject != null)
        {
            dragObject = null;
            connectionTargets.Clear();
            startDrag?.Invoke(false);
        }
    }
    private RaycastHit2D GetRaycastHit(int layer)
    {
        Ray ray = cam.ScreenPointToRay(playerInput.Main.MousePosition.ReadValue<Vector2>());
        return Physics2D.GetRayIntersection(ray, Mathf.Infinity, layer);
    }
    private RaycastHit2D[] GetRaycastHits(int layer)
    {
        Ray ray = cam.ScreenPointToRay(playerInput.Main.MousePosition.ReadValue<Vector2>());
        return Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity, layer);
    }
    public Vector2 MousePosition()
    {
        var mpp = playerInput.Main.MousePosition.ReadValue<Vector2>();
        var mousePoint = new Vector3(mpp.x, mpp.y, 10);
        return cam.ScreenToWorldPoint(mousePoint);
    }
    private void MouseMove()
    {
        if (dragObject != null)
        {
            dragObject.transform.position = MousePosition();

            var hits = GetRaycastHits(connectingMask);
            var hitObjects = new List<ConnectionsController>();
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.parent.TryGetComponent(out ConnectionsController hitConnect))
                    hitObjects.Add(hit.transform.parent.GetComponent<ConnectionsController>());
            }
            if (hits.Length > 0 && connectionTargets.Count == 0)
            {
                AddConnections(hitObjects);
            }
            else if (connectionTargets.Count != hits.Length)
            {
                RemoveExcessConnections(hitObjects);
                AddConnections(hitObjects);
            }
        }

    }
    private void AddConnections(List<ConnectionsController> hitObjects)
    {
        foreach (ConnectionsController hitConnect in hitObjects)
        {
            if (!connectionTargets.Contains(hitConnect))
            {
                dragObject.StartDrawLine(hitConnect);
                connectionTargets.Add(hitConnect);
            }

        }
    }
    private void RemoveExcessConnections(List<ConnectionsController> hitObjects)
    {
        for (int i = 0; i < connectionTargets.Count; i++)
        {
            if (!hitObjects.Contains(connectionTargets[i]))
            {
                dragObject.CancelDrawLine(connectionTargets[i]);
                connectionTargets.Remove(connectionTargets[i]);
            }
        }
    }
}

