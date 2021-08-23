using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
public class NodeController : MonoBehaviour
{

    [SerializeField] private string _nodeId;

    public NodeOptions Options;

    private SpriteRenderer _spriteRenderer;

    private ConnectionsController _connections;

    public TaskHolder Task;
    private void Awake()
    {
        DataLoader.instance.OnNodeDataLoaded += UpdateOptions;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _connections = GetComponentInChildren<ConnectionsController>();
    }
    private void UpdateOptions(NodesData nodesData)
    {
        Options = nodesData.NodeOptionsList.Find(x => x.Id == _nodeId);
        SetGameObject();
        _connections.SetConnectionArea();
        Task = TaskManager.GetTask(Options.BaseTask, this);
    }
    private void SetGameObject()
    {
        gameObject.name = _nodeId;
        _spriteRenderer.sprite = Options.Icon;
    }
}
