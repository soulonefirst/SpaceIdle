using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class TopMenuController : MonoBehaviour, IPointerClickHandler
{
    private List<GameObject> _activeItems = new List<GameObject>();
    private Dictionary<Text, Slider> _sliders = new Dictionary<Text, Slider>();
    private ObjectPool<GameObject> _objectPool;
    private bool _isMunuShown;

    private void Start()
    {
        LoadOptions();
    }
    private async void LoadOptions()
    {
        while (!LoadAssetBundle.assetsLoaded)
        {
            await Task.Yield(); 
        }
         var itemPrefab =   LoadAssetBundle.GetPrefab("ResourceMenuItem");
        _objectPool = new ObjectPool<GameObject>(itemPrefab, transform.GetChild(0));

        CreateMenuItem(itemPrefab,"Ore XP ");
        CreateMenuItem(itemPrefab, "Crystal XP ");
    }
    private void CreateMenuItem(GameObject prefab, string text)
    {
        var obj = Instantiate(prefab);
        var t = obj.transform.GetChild(0).GetComponent<Text>();
        t.text = text;
        var s = obj.transform.GetChild(1).GetComponent<Slider>();
        _sliders.Add(t, s);
        _objectPool.Push(obj);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isMunuShown)
            _isMunuShown = ShowMenu();
        else
            _isMunuShown = CloseMenu();
    }
    private bool ShowMenu()
    {
        for(int i = 1; i <= _objectPool.Count(); i++)
        {                                     
            _activeItems.Add(_objectPool.Pop());
        }
        return true;
    }
    private bool CloseMenu()
    {
        for (int i = _activeItems.Count-1; i >= 0 ; i--)
        {
            _objectPool.Push(_activeItems[i]);
        }
        return false;
    }
    private void Update()
    {

    }
}
