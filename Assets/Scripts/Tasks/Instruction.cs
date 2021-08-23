using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System;

public interface IInstruction
{
    bool IsExecuting { get; }
    bool IsPaused { get; }

    Instruction Execute(MonoBehaviour parent);
    void Pause();
    void Resume();
    void Terminate();

    event Action<Instruction> Started;
    event Action<Instruction> Paused;
    event Action<Instruction> Terminated;
    event Action<Instruction> Done;
}

public abstract class Instruction : IEnumerator, IInstruction
{
    private Instruction _current;
    object IEnumerator.Current => _current;

    private object routine;
    protected bool IsLooped;
    public MonoBehaviour Parent { get; private set; }

    public bool IsExecuting { get; private set; }
    public bool IsPaused { get; private set; }
    private bool IsStopped { get; set; } = true;

    public event Action<Instruction> Started;
    public event Action<Instruction> Paused;
    public event Action<Instruction> Terminated;
    public event Action<Instruction> Done;

    protected Instruction(MonoBehaviour parent) => Parent = parent;
    void IEnumerator.Reset()
    {
        IsPaused = false;
        IsStopped = false;
        IsExecuting = false;
        routine = null;
    }

    bool IEnumerator.MoveNext()
    {
        if (IsStopped)
        {
            if (IsLooped)
            {
                if (!OnStarted())
                    return true;
                IsStopped = false;
                return true;
            }
            (this as IEnumerator).Reset();
            return false;
        }

        if (!IsExecuting)
        {
            IsExecuting = true;
            routine = new object();

            OnStarted();
            Started?.Invoke(this);
        }

        if (_current != null)
            return true;

        if (IsPaused)
        {
            return true;
        }

        if (!Update())
        {
            OnDone();
            Done?.Invoke(this);

            IsStopped = true;
        }

        return true;
    }


    public void Pause()
    {
        if (IsExecuting && !IsPaused)
        {
            IsPaused = true;

            OnPaused();
            Paused?.Invoke(this);
        }
    }

    public void Resume()
    {
        IsPaused = false;
        OnResumed();
    }

    public void Terminate()
    {
        if (Stop())
        {
            OnTerminated();
            Terminated?.Invoke(this);
        }
    }

    private bool Stop()
    {
        if (IsExecuting)
        {
            if (routine is Coroutine)
                Parent.StopCoroutine(routine as Coroutine);

            (this as IEnumerator).Reset();

            return IsStopped = true;
        }

        return false;
    }
    public IEnumerator AddChildInstructuion(Instruction instruction)
    {
        _current = instruction;
        yield return instruction;
        _current = null;
    }
    public Instruction Execute()
    {
        if (_current != null)
        {
            Debug.LogWarning($"Instruction { GetType().Name} is currently waiting for another one and can't be stared right now.");
            return this;
        }

        if (!IsExecuting)
        {
            IsExecuting = true;
            routine = Parent.StartCoroutine(this);
            OnStarted();
            return this;
        }
        if (IsPaused)
        {
            Resume();
            return this;
        }
        Debug.LogWarning($"Instruction { GetType().Name} is already executing.");
        return this;
    }

    public Instruction Execute(MonoBehaviour parent)
    {
        if (_current != null)
        {
            Debug.LogWarning($"Instruction { GetType().Name} is currently waiting for another one and can't be stared right now.");
            return this;
        }

        if (!IsExecuting)
        {
            IsExecuting = true;
            routine = (Parent = parent).StartCoroutine(this);
            OnStarted();
            return this;
        }
        if (IsPaused)
        {
            Resume();
            return this;
        }
        Debug.LogWarning($"Instruction { GetType().Name} is already executing.");
        return this;
    }

    public void Reset()
    {
        Terminate();

        Started = null;
        Paused = null;
        Terminated = null;
        Done = null;
    }

    protected virtual bool OnStarted() { return true; }
    protected virtual void OnPaused() { }
    protected virtual void OnResumed() { }
    protected virtual void OnTerminated() { }
    protected virtual void OnDone() { }
    protected abstract bool Update();
}
public sealed class Wait : Instruction
{
    public float Delay { get; set; }
    private float startTime;

    protected override bool Update()
    {
        return Time.time - startTime < Delay;
    }

    public Wait(MonoBehaviour parent) : base(parent) { }

    public Wait(float delay, MonoBehaviour parent) : base(parent)
    {
        Delay = delay;
    }

    protected override bool OnStarted()
    {
        startTime = Time.time;
        return true;
    }

    public Instruction Execute(float delay)
    {
        Delay = delay;

        return base.Execute();
    }
    protected override void OnPaused()
    {
        Delay -= Time.time - startTime;
    }

    protected override void OnResumed()
    {
        startTime = Time.time;
    }
}




