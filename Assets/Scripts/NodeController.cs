using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class NodeController : MonoBehaviour
{
    [SerializeField] private string nodeId;

    public NodeOptions options;

    [SerializeField] private bool taskComplete;

    private Transform progressBar;
    private SpriteRenderer spriteRenderer;
    private ConnectionsController connections;
    private void Awake()
    {
        GoogleSheetLoader.OnProcessData += UpdateOptions;
    }

    private void Start()
    {
        progressBar = transform.GetChild(0).Find("Bar").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        connections = GetComponentInChildren<ConnectionsController>();
    }
    private void UpdateOptions(NodesData nodesData)
    {
        options = nodesData.NodeOptionsList.Find(x => x.Id == nodeId);
        SetGameObject();
        connections.SetConnectionArea(options.Draggable);
    }
    private void SetGameObject()
    {
        gameObject.name = nodeId;
        spriteRenderer.sprite = options.Icon;
        if (options.Draggable)
            gameObject.layer = 6;
    }
   
    public void TaskStart()
    {
        taskComplete = false;
        StartCoroutine(TaskProcess());
    }
    public void TaskComplete()
    {
        taskComplete = true;
    }
    private bool CheckRequariments()
    {
        return true;
    }
    private IEnumerator TaskProcess()
    {
        float newScaleX = progressBar.localScale.x + (1 / options.ProduceSpeed / 10);
        if (newScaleX > 1.0f)
            newScaleX = 1.0f;
        progressBar.localScale = new Vector3(newScaleX, 1, 1);
        yield return new WaitForSeconds(0.1f);
        if(newScaleX == 1.0f)
        {
            progressBar.localScale = new Vector3(0, 1, 1);
            TaskComplete();
        }
    }

}
