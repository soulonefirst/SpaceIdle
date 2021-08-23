using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InputsController : Singleton<InputsController>
{
    [SerializeField] private List<ConnectionsController> _connectionTargets = new List<ConnectionsController>();
    [SerializeField] private ConnectionsController _dragObject;


    private PlayerInputs _playerInput;
    private Camera _cam;

    private int _draggableMask;
    private int _connectingMask;

    public static event Action<ConnectionsController> StartDrag;
    private void Start()
    {
        _cam = Camera.main;
        _draggableMask = LayerMask.GetMask("Draggable");
        _connectingMask = LayerMask.GetMask("ConnectingArea");
    }
    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInputs();
        }
        _playerInput.Main.Enable();

        _playerInput.Main.LeftClick.started += x => LeftClickStart(GetRaycastHit(_draggableMask));
        _playerInput.Main.LeftClick.canceled += x => LeftClickEnd(GetRaycastHits(_connectingMask));
        _playerInput.Main.Scroll.performed += x => CameraMoveController.instance.OnScroll(x);
        _playerInput.Main.MousePosition.performed += x => MouseMove(x);
    }

    private void OnDisable()
    {
        _playerInput.Main.Disable();
    }
    private void LeftClickStart(RaycastHit2D hit)
    {
        if (hit && hit.transform.TryGetComponent(out ConnectionsController hitObject))
        {
            _dragObject = hitObject;
            _dragObject.ActivateConnectedLines(true);
            _dragObject.ShowConnectionArea(_dragObject);
            StartDrag?.Invoke(_dragObject);
        }else
            _playerInput.Main.SetCallbacks(CameraMoveController.instance);
    }
    private void LeftClickEnd(RaycastHit2D[] hits)
    {
        if (_dragObject != null)
        {
            _dragObject.ActivateConnectedLines(false);
            _dragObject = null;
            _connectionTargets.Clear();
            StartDrag?.Invoke(null);
        }
        else
            _playerInput.Main.SetCallbacks(null);
    }
    private RaycastHit2D GetRaycastHit(int layer)
    {
        Ray ray = _cam.ScreenPointToRay(_playerInput.Main.MousePosition.ReadValue<Vector2>());
        return Physics2D.GetRayIntersection(ray, Mathf.Infinity, layer);
    }
    private RaycastHit2D[] GetRaycastHits(int layer)
    {
        Ray ray = _cam.ScreenPointToRay(_playerInput.Main.MousePosition.ReadValue<Vector2>());
        return Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity, layer);
    }
    private Vector2 MousePosition(Vector2 mousePosition)
    {
        var mousePoint = new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.z * -1);
        return _cam.ScreenToWorldPoint(mousePoint);
    }
    private void MouseMove(CallbackContext callback)
    {
        if (_dragObject != null)
        {
            _dragObject.transform.position = MousePosition(callback.ReadValue<Vector2>());

            var hits = GetRaycastHits(_connectingMask);
            var hitObjects = new List<ConnectionsController>();
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.parent.TryGetComponent(out ConnectionsController hitConnect))
                    hitObjects.Add(hit.transform.parent.GetComponent<ConnectionsController>());
            }

            if (_connectionTargets.Count != hits.Length)
            {
                CheckConnections(hitObjects);
                AddConnections(hitObjects);
            }
        }

    }
    private void AddConnections(List<ConnectionsController> hitObjects)
    {
        foreach (ConnectionsController hitObject in hitObjects)
        {
            if (!_connectionTargets.Contains(hitObject))
            {
                _dragObject.AddConnention(hitObject);
                _connectionTargets.Add(hitObject);
            }

        }
    }
    private void CheckConnections(List<ConnectionsController> hitObjects)
    {
        for (int i = 0; i < _connectionTargets.Count; i++)
        {
            if (!hitObjects.Contains(_connectionTargets[i]))
            {
                _connectionTargets.Remove(_connectionTargets[i]);
            }
        }
        _dragObject.DeleteExcessConnection(hitObjects);
    }
}

