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
    event Action<Instruction> Cancelled;
    event Action<Instruction> Done;
}

public abstract class Instruction : IEnumerator, IInstruction
{
    private Instruction current;
    object IEnumerator.Current => current;

    private object routine;
    protected bool IsLooped;
    private float _timeToWait;
    public MonoBehaviour Parent { get; private set; }

    public bool IsExecuting { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsWaiting { get; private set; }

    private bool IsStopped { get; set; }

    public event Action<Instruction> Started;
    public event Action<Instruction> Paused;
    public event Action<Instruction> Terminated;
    public event Action<Instruction> Done;
    public event Action<Instruction> Cancelled;

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
                OnStarted();
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

        if (current != null)
            return true;

        if (IsPaused)
        {
            return true;
        }
        if (IsWaiting)
        {
            if (Time.time >= _timeToWait)
                IsWaiting = false;
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
    public void Wait(float t)
    {
        _timeToWait = Time.time + t;
        IsWaiting = true;
    }

    public Instruction Execute()
    {
        if (current != null)
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
        if (current != null)
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

    protected virtual void OnStarted() { }
    protected virtual void OnPaused() { }
    protected virtual void OnResumed() { }
    protected virtual void OnTerminated() { }
    protected virtual void OnDone() { }

    protected abstract bool Update();
}





