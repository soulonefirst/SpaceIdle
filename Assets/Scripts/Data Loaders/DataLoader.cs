using System;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CVSLoader), typeof(SheetProcessor),typeof(LoadAssetBundle))]
public class DataLoader : Singleton<DataLoader>
{
    public event Action<NodesData> OnNodeDataLoaded;


    public event Action OnDataLoaded;

    [SerializeField] private string _sheetId;
    [SerializeField] private string[] _pagesId; 
    [SerializeField] private NodesData _nodeData;
    [SerializeField] private int[] _oreData;

    private CVSLoader _cvsLoader;
    private SheetProcessor _sheetProcessor;
    private LoadAssetBundle _loadAssetBundle;

    protected override void Awake()
    {
        _cvsLoader = GetComponent<CVSLoader>();
        _sheetProcessor = GetComponent<SheetProcessor>();
        _loadAssetBundle = GetComponent<LoadAssetBundle>();
        LoadData();
    }
    protected void Start()
    {
        OnDataLoaded?.Invoke();        
    }
    private void LoadData()
    {
        _loadAssetBundle.LoadAssetsBundles();
        foreach(string page in _pagesId)
        {
            _cvsLoader.DownloadTable(_sheetId, page, OnRawCVSLoaded); 
        }
    }

    private void OnRawCVSLoaded(string rawCVSText, string pageId)
    {
        switch (pageId)
        {
            case "Nodes":
                _nodeData = _sheetProcessor.ProcessNodeData(rawCVSText);
                OnNodeDataLoaded?.Invoke(_nodeData);
                break;
            case "OreXP":
                _sheetProcessor.ProcessOreXP(rawCVSText);
                break;
        }
    }

}
