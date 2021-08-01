using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LoadAssetBundle : MonoBehaviour
{

    public static bool assetsLoaded;
    private static List<GameObject> prefabs = new List<GameObject>();
    private static Dictionary<string,Sprite> sprites = new Dictionary<string, Sprite>();
    [SerializeField] private string[] assetBundleNames;

    private void Awake()
    {
        LoadAssetsBundles();
    }
    public void LoadAssetsBundles()
    {
        foreach (string s in assetBundleNames)
        {
            StartCoroutine(LoadAsset(s));            
        }
    }
    public static Sprite GetSprite(string name)
    {
        if(sprites.TryGetValue(name, out Sprite sprite))
        {
            return sprite;
        } else
        Debug.LogError("Can't find sprite named " + name);
        return null;
    }
    public static GameObject GetPrefab(string name)
    {
        foreach (GameObject gameObject in prefabs)
        {
            if (gameObject.name == name)
            {
                return gameObject;
            }
        }
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

        //Load the Asset (Use Texture2D since it's a Texture. Use GameObject if prefab)
        AssetBundleRequest assetsReq = asseBundle.LoadAllAssetsAsync<object>();
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
                    prefabs.Add(o as GameObject);
                }
                break;
        }
        if (assetBundleName == assetBundleNames[assetBundleNames.Length - 1])
            assetsLoaded = true;
        Debug.Log(assetBundleName + " Loaded");
    }
}
