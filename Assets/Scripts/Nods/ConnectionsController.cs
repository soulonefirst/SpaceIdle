using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public struct OutputConnection
{
    public ConnectionsController Target;
    public LineController Line;
}
public class ConnectionsController : MonoBehaviour
{
    [SerializeField] private List<OutputConnection> _outputConnections = new List<OutputConnection>();
    [SerializeField] private List<ConnectionsController> _inputConnections = new List<ConnectionsController>();

    private GameObject _line;
    private NodeController _node;
    private GameObject _circle;

    private void Start()
    {
        _node = GetComponent<NodeController>();
        _circle = transform.GetChild(1).gameObject;
        _line = LoadAssetBundle.GetPrefab("Line");
    }
    public void ChangeInputConnection(ConnectionsController input)
    {
        if (!_inputConnections.Contains(input))
        {
            _inputConnections.Add(input);
            OnConnectionsIncrease(input);
        }
        else
        {
            _inputConnections.Remove(input);
            OnConnectionsDecreace();
        }
    }
    private void OnConnectionsIncrease(ConnectionsController input)
    {
        _node.Task?.Execute();
        input._node.Task?.Execute();
    }
    private void OnConnectionsDecreace()
    {
        if (_inputConnections.Count == 0)
        {
            _node.Task?.Terminate();
        }
    }
    public void OnOutputConnectionDecreace()
    {
        if (_outputConnections.Count == 0)
        {
            _node.Task?.Terminate();
        }
    }
    public void SetConnectionArea(bool dragablle)
    {
        _circle.GetComponent<SpriteRenderer>().color = _node.Options.Color;
        InputsController.StartDrag += ShowConnectionArea;
    }
    public void ShowConnectionArea(ConnectionsController dragObject)
    {
        if (dragObject == null)
            _circle.SetActive(false);
        else if (CheckRequirements(dragObject) || dragObject == this)
        {
            _circle.SetActive(true);
        }
    }
    public bool CheckRequirements(ConnectionsController anotherNode) =>
        _node.Options.Requirements.Contains(anotherNode._node.Options.BaseTask.ToString());

    public void AddConnention(ConnectionsController hitObject)
    {
        if (!CheckContainConnection(hitObject))
        {
            var gO = Instantiate(_line, transform);
            var LC = gO.GetComponent<LineController>();
            LC.BindPoints(hitObject.transform);

            _outputConnections.Add(new OutputConnection { Target = hitObject, Line = LC });
            hitObject.ChangeInputConnection(this);
        }
    }
    public bool CheckContainConnection(ConnectionsController hitObject) =>
        _outputConnections.Select(x => x.Target).Contains(hitObject);
    public void DeleteExcessConnection(List<ConnectionsController> hitObjects)
    {
        for (int i = 0; i < _outputConnections.Count; i++)
        {
            if (!hitObjects.Contains(_outputConnections[i].Target))
            {
                Destroy(_outputConnections[i].Line.gameObject);

                _outputConnections[i].Target.ChangeInputConnection(this);
                _outputConnections.Remove(_outputConnections[i]);
                OnOutputConnectionDecreace();
            }
        }
    }
    public void ActivateConnectedLines(bool b)
    {
        if (_outputConnections.Count > 0)
        {
            foreach (OutputConnection connection in _outputConnections)
            {
                connection.Line.enabled = b;
            }
        }
    }
}