using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class LoadAssetBundle : Singleton<LoadAssetBundle>
{
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    private Dictionary<string,Sprite> sprites = new Dictionary<string, Sprite>();
    [SerializeField] private string[] assetBundleNames;

    public void LoadAssetsBundles()
    {
        foreach (string s in assetBundleNames)
        {
            StartCoroutine(LoadAsset(s));            
        }
    }
    public Sprite GetSprite(string name)
    {        
        if(sprites.TryGetValue(name, out Sprite sprite))
        {
            return sprite;
        } else
        Debug.LogError("Can't find sprite named " + name);
        return null;
    }
    public GameObject GetPrefab(string name)
    {
        GameObject p;
        if(prefabs.TryGetValue(name, out p))
        {
            return p;
        }else
        Debug.LogError("Can't find prefab named " + name);
        return null;
    }
    private  IEnumerator LoadAsset(string assetBundleName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles");

        filePath = System.IO.Path.Combine(filePath, assetBundleName);

        //Load AssetBundle
        var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
        yield return assetBundleCreateRequest;

        AssetBundle asseBundle = assetBundleCreateRequest.assetBundle;

        AssetBundleRequest assetsReq = asseBundle.LoadAllAssetsAsync<Object>();
        yield return assetsReq;
        switch (assetBundleName)
        {
            case "sprites":
                foreach (object o in assetsReq.allAssets)
                {
                    if (o.GetType() == typeof(Sprite))
                    {
                        Sprite sprite = o as Sprite;
                        sprites.Add(sprite.name, sprite);
                    }
                }
                break;
            case "prefabs":
                foreach (object o in assetsReq.allAssets)
                {
                    GameObject gO = o as GameObject;
                    prefabs.Add(gO.name,gO);
                }
                break;
        }
        if (assetBundleName == assetBundleNames[assetBundleNames.Length - 1])
            
        Debug.Log(assetBundleName + " Loaded");
    }
}
