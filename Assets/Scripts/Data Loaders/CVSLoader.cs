using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CVSLoader : Singleton<CVSLoader>
{
    private bool _debug = true;
    private const string url = "https://docs.google.com/spreadsheets/d/*/gviz/tq?tqx=out:csv&sheet=";

    public void DownloadTable(string sheetId, string pageId, Action<string,string> onSheetLoadedAction)
    {
        string actualUrl = url.Replace("*", sheetId);
        actualUrl += pageId;
        StartCoroutine(DownloadRawCvsTable(actualUrl, pageId, onSheetLoadedAction));
    }

    private IEnumerator DownloadRawCvsTable(string actualUrl,string pageId, Action<string,string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(actualUrl))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                if (_debug)
                {
                    Debug.Log("Successful download " + pageId);
                }

                callback(request.downloadHandler.text, pageId);
            }
            
        }
        yield return null;
    }
}
