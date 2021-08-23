
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskName
{
    OreWork,
    CreateOre,
    CreateCrystal,
    CrystalWork
}
public static class TaskManager
{
    public static TaskHolder GetTask(TaskName task, NodeController node)
    {
        switch (task)
        {
            case TaskName.CreateOre:
                return new CreateOre(node);
            case TaskName.OreWork:
                return new OreWork(node);

            default: return null;
        }
    }
}
public sealed class CreateOre : TaskHolder
{
    public CreateOre(MonoBehaviour parent) : base(parent) { }
    protected override void OnDone()
    {
        Vector2 pPos = Parent.transform.position;
        base.OnDone();
        GameObject.Instantiate(LoadAssetBundle.instance.GetPrefab("OrePiece"),
            new Vector3(pPos.x + Random.Range(-1.0f, 1.0f), pPos.y + Random.Range(-1.0f, 1.0f), 0),
            Quaternion.identity,
            Parent.transform);
        XPManager.instance.ReciveOreXP();
    }
}
public sealed class OreWork : TaskHolder
{
    private int _oreCount;
    private RaycastHit2D _oreCollider;
    private LayerMask _oreLayer;
    private Transform _areaTransform;
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
        var oPos = _oreCollider.transform.position;

        _oreCollider.transform.position -= (oPos -aPos) / _nodeOptions.Stats.ProduceSpeed / 10;
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
        GameObject.Destroy(_oreCollider.transform.gameObject);
    }
}
public abstract class TaskHolder : Instruction
{
    private GameObject _progressBar;
    private Transform _barTrans;
    protected NodeOptions _nodeOptions;
    public TaskHolder(MonoBehaviour parent) : base(parent)
    {
        _progressBar = parent.transform.GetChild(0).gameObject;
        _barTrans = _progressBar.transform.Find("Bar").transform;
        var p = parent as NodeController;
        _nodeOptions = p.Options;
        IsLooped = true;
    }
    protected virtual void ResetBar()
    {
        _barTrans.localScale = Vector3.zero;
    }
    protected override bool OnStarted()
    {
        _progressBar.SetActive(true);
        return true;
    }
    protected override bool Update()
    {
        Parent.StartCoroutine(AddChildInstructuion(new Wait(0.1f, Parent)));
        float newScaleX = _barTrans.localScale.x + (1 / _nodeOptions.Stats.ProduceSpeed / 10);
        _barTrans.localScale = new Vector3(newScaleX, 1, 1);
        if (newScaleX >= 1.0f)
        {
            return false;
        }
        return true;
    }
    protected override void OnDone()
    {
        ResetBar();
        _progressBar.SetActive(false);
    }
    protected override void OnTerminated()
    {
        ResetBar();
        _progressBar.SetActive(false);
    }
}