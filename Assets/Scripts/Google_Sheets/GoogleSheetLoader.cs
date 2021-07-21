using System;
using UnityEngine;

[RequireComponent(typeof(CVSLoader), typeof(SheetProcessor))]
public class GoogleSheetLoader : MonoBehaviour
{
    public static event Action<NodesData> OnProcessData;
    
    [SerializeField] private string _sheetId;
    [SerializeField] private string[] _pagesId; 
    [SerializeField] private NodesData _data;

    private CVSLoader _cvsLoader;
    private SheetProcessor _sheetProcessor;

    private void Start()
    {
        _cvsLoader = GetComponent<CVSLoader>();
        _sheetProcessor = GetComponent<SheetProcessor>();
        DownloadTable();
    }

    private void DownloadTable()
    {
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
                _data = _sheetProcessor.ProcessNodeData(rawCVSText);
                OnProcessData?.Invoke(_data);
                break;
            case "Nodes2":
                _sheetProcessor.Test(rawCVSText);
                break;
        }
    }

}
