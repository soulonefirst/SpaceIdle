using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class NodeController : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private string nodeId;

    public Options options;

    [SerializeField]
    private bool inProgress;

    private Transform progressBar;
    private TextMesh textMesh;
    private ConnectionsController connections;
    private void Awake()
    {
        GoogleSheetLoader.OnProcessData += UpdateOptions;
    }

    private void Start()
    {
        progressBar = transform.GetChild(0).Find("Bar").transform;
        textMesh = GetComponent<TextMesh>();
        connections = GetComponentInChildren<ConnectionsController>();
    }
    private void UpdateOptions(NodesData nodesData)
    {
        options = new Options(nodesData.NodeOptionsList.Find(x => x.Id == nodeId));
        SetGameObject();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
        foreach(GameObject gO in eventData.hovered)
        {
            if(gO.name == "Output")
            {
                connections.StartDrawLine().lineIsDraw = true;
                break;
            }else if(gO.name == nodeId)
            {

            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (GameObject gO in eventData.hovered)
        {
            if (gO.name == "Input" && connections.lineIsDraw==true)
            {
                connections.AddConnection(eventData.pointerPress).lineIsDraw = false;
                break;
            }
        }
        if (connections.lineIsDraw == true)
        {
            connections.StopDrawLine().lineIsDraw = false;
        }
    }
    private void SetGameObject()
    {
        gameObject.name = nodeId;
        textMesh.text = nodeId;
        //Icon
        textMesh.color = options.Color;
    }
   
    public void TaskStart()
    {
        StartCoroutine(TaskProcess());
    }
    public void TaskComplete()
    {

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
        if (CheckRequariments())
        StartCoroutine(TaskProcess());
    }

}
