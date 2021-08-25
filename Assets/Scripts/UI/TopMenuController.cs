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

    private Text _overallXPText;
    private void Start()
    {
        DataLoader.Instance.OnDataLoaded += LoadOptions;
        _overallXPText = GameObject.Find("OverallXPAmount").GetComponent<Text>();
        Debug.Log(_overallXPText.gameObject.name);
    }
    private void Update()
    {
        _overallXPText.text = "";
    }
    private void LoadOptions()
    {
         var itemPrefab =   LoadAssetBundle.Instance.GetPrefab("ResourceMenuItem");
        _objectPool = new ObjectPool<GameObject>(itemPrefab, transform.GetChild(0));

        CreateMenuItem(itemPrefab,"Ore XP ");
        CreateMenuItem(itemPrefab, "Crystal XP ");
    }
    private void CreateMenuItem(GameObject prefab, string text)
    {
        var obj = Instantiate(prefab);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-80 * _objectPool.Count());
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
            _activeItems.RemoveAt(i);
        }
        return false;
    }
}
