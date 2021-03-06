using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class OutputConnection
{
    public ConnectionsController Target;
    public LineController Line;
}
public class ConnectionsController : MonoBehaviour
{
    [SerializeField] private List<OutputConnection> _outputConnections = new List<OutputConnection>();
    [SerializeField] private List<ConnectionsController> _inputConnections = new List<ConnectionsController>();

    private static ObjectPool<GameObject> _linePool;

    private GameObject _linePrefab;
    private NodeController _node;
    private GameObject _circle;

    private void Start()
    {
        _node = GetComponent<NodeController>();
        _circle = transform.GetChild(1).gameObject;
        DataLoader.Instance.OnDataLoaded += SetPool;
    }
    private async void SetPool()
    {
        _linePrefab =  await Addressables.LoadAssetAsync<GameObject>("Line").Task;
        _linePool = new ObjectPool<GameObject>(_linePrefab, GameObject.Find("ObjectPool").transform, 0);
    }
    private void ChangeInputConnection(ConnectionsController input)
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
        _node.Task.Execute();
        input._node.Task.Execute();
    }
    private void OnConnectionsDecreace()
    {
        if (_inputConnections.Count == 0)
        {
            _node.Task.Stop();
        }
    }
    private void OnOutputConnectionDecreace()
    {
        if (_outputConnections.Count == 0)
        {
            _node.Task.Stop();
        }
    }
    public void SetConnectionArea()
    {
        _circle.GetComponent<SpriteRenderer>().color = _node.Options.Color;
        var scale = _node.Options.Stats.ConnectionAreaSize;
        _circle.transform.localScale = new Vector3(scale, scale, 1);
        InputsController.OnStartDrag += ShowConnectionArea;
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

    private bool CheckRequirements(ConnectionsController anotherNode) =>
        _node.Options.Requirements.Contains(anotherNode._node.Options.BaseTask);

    public void AddConnention(ConnectionsController hitObject)
    {
        if (CheckContainConnection(hitObject)) return;
        
        var gO = _linePool.Pop();
        gO.transform.SetParent(transform);
        var LC = gO.GetComponent<LineController>();
        LC.BindPoints(transform,hitObject.transform);

        _outputConnections.Add(new OutputConnection { Target = hitObject, Line = LC });
        hitObject.ChangeInputConnection(this);
    }

    private bool CheckContainConnection(ConnectionsController hitObject) =>
        _outputConnections.Select(x => x.Target).Contains(hitObject);

    public void DeleteExcessConnection(ConnectionsController excessConnection)
    {
        var outputConnection = _outputConnections.FirstOrDefault(x => x.Target == excessConnection);
        if (outputConnection != null)
        {
            _linePool.Push(outputConnection.Line.gameObject);

            outputConnection.Target.ChangeInputConnection(this);
            _outputConnections.Remove(outputConnection);
        }

        OnOutputConnectionDecreace();
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
