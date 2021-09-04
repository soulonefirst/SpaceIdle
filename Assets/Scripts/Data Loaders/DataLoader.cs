using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SheetProcessor), typeof(CVSLoader))]
public class DataLoader : Singleton<DataLoader>
{
    public event Action<NodesData> OnNodeDataLoaded;

    public event Action OnDataLoaded;

    [SerializeField] private string sheetId;
    [SerializeField] private string[] pagesId;
    [SerializeField] private NodesData nodeData;
    [SerializeField] private int[] oreData;

    private CVSLoader _cvsLoader;
    private SheetProcessor _sheetProcessor;
    private int pageCounter;

    protected override void Awake()
    {
        _cvsLoader = GetComponent<CVSLoader>();
        _sheetProcessor = GetComponent<SheetProcessor>();
       LoadData();
    }
    private void LoadData()
    {
        foreach(string page in pagesId)
        {
            _cvsLoader.DownloadTable(sheetId, page, OnRawCVSLoaded); 
        }
    }

    private void Loading()
    {
        pageCounter++;
        if (pageCounter == pagesId.Length)
        {
            OnDataLoaded?.Invoke();
        }
    }
    private void OnRawCVSLoaded(string rawCvsText, string pageId)
    {
        switch (pageId)
        {
            case "Nodes":
                nodeData = _sheetProcessor.ProcessNodeData(rawCvsText);
                OnNodeDataLoaded?.Invoke(nodeData);
                break;
            case "OreXP":
                _sheetProcessor.ProcessOreXP(rawCvsText);
                break;
        }
        Loading();
    }

}
