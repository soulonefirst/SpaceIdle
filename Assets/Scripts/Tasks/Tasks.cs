using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class TaskHolder : Instruction
{
    private readonly GameObject _progressBar;
    private readonly Transform _barTrans;
    protected readonly NodeOptions NodeOptions;

    protected TaskHolder(MonoBehaviour parent) : base(parent)
    {
        _progressBar = parent.transform.GetChild(0).gameObject;
        _barTrans = _progressBar.transform.Find("Bar").transform;
        var p = parent as NodeController;
        NodeOptions = p.Options;
        IsLooped = true;
    }
    protected virtual void ResetBar()
    {
        _barTrans.localScale = Vector3.zero;    
    }

    protected override void OnDone()
    {
        ResetBar();
        _progressBar.SetActive(false);
    }

    protected override bool OnStarted()
    {
        _progressBar.SetActive(true);

        return true;
    }

    protected override bool Update()
    {
        Parent.StartCoroutine(AddChildInstructuion(new Wait(0.1f, Parent)));
        var newScaleX = _barTrans.localScale.x + (1 / NodeOptions.Stats.ProduceSpeed / 10);
        _barTrans.localScale = new Vector3(newScaleX, 1, 1);
        return !(newScaleX >= 1.0f);
    }

    protected override void OnTerminated()
    {
        ResetBar();
        _progressBar.SetActive(false);
    }
}

public sealed class CreateOre : TaskHolder
{
    private GameObject _orePiece;

    public CreateOre(MonoBehaviour parent) : base(parent)
    {
        _orePiece = Addressables.LoadAssetAsync<GameObject>("OrePiece").WaitForCompletion();
    }
    protected override void OnDone()
    {
        Vector2 pPos = Parent.transform.position;
        base.OnDone();
        Object.Instantiate(_orePiece,
            new Vector3(pPos.x + Random.Range(-1.0f, 1.0f), pPos.y + Random.Range(-1.0f, 1.0f), 0),
            Quaternion.identity,
            Parent.transform);
        XPManager.Instance.ReciveOreXp();
    }
}

public sealed class OreWork : TaskHolder
{
    private int _oreCount;
    private RaycastHit2D _oreCollider;
    private readonly LayerMask _oreLayer;
    private readonly Transform _areaTransform;

    public OreWork(MonoBehaviour parent) : base(parent)
    {
        _areaTransform = parent.transform.GetChild(1).transform;
        _oreLayer = LayerMask.GetMask("Ore");
    }

    protected override bool OnStarted()
    {
        if (_oreCollider == false)
        {
            _oreCollider = Physics2D.CircleCast(_areaTransform.position, _areaTransform.localScale.x / 2, Vector2.zero, Mathf.Infinity, _oreLayer);
            if (_oreCollider == false)
            {
                Parent.StartCoroutine(AddChildInstructuion(new Wait(1f, Parent)));
                return false;
            }
            else
                _oreCollider.transform.gameObject.layer = 0;
        }

        return base.OnStarted();        
    }

    protected override bool Update()
    {
        
        var aPos = _areaTransform.position;
        var position = _oreCollider.transform.position;
        var oPos = position;

        position -= (oPos -aPos) / NodeOptions.Stats.ProduceSpeed / 10;
        _oreCollider.transform.position = position;
        return base.Update();
    }
    protected override void OnTerminated()
    {
        base.OnTerminated();
        if(_oreCollider == true)
        {
            _oreCollider.transform.gameObject.layer = LayerMask.NameToLayer("Ore");
            _oreCollider = new RaycastHit2D();
        }
        ResetBar();
    }

    protected override void OnDone()
    {
        base.OnDone();
        Object.Destroy(_oreCollider.transform.gameObject);
    }
}