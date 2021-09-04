using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System;
using Instructions;

public abstract class Instruction : IEnumerator, IInstruction<object>
{
    public object Value { get; set; }
    protected Instruction _current;
    object IEnumerator.Current => _current;

    private object _routine;
    protected MonoBehaviour Parent { get; private set; }

    public bool IsExecuting { get; private set; }
    public bool IsPaused { get; private set; }
    private bool IsStopped { get; set; }


    public event Action<Instruction> OnStarted;
    public event Action<Instruction> OnPaused;
    public event Action<Instruction> OnStopped;
    public event Action<Instruction> OnDone;

    protected Instruction(MonoBehaviour parent) => Parent = parent;
    void IEnumerator.Reset()
    {
        IsPaused = false;
        IsStopped = false;
        IsExecuting = false;
        _routine = null;
        _current = null;
    }

    bool IEnumerator.MoveNext()
    {
        if (IsStopped)
        {
            Stop();
            return false;
        }
        if (!IsExecuting)
        {
            IsExecuting = true;
            _routine = new object();

            if(!Started()) return false;
            OnStarted?.Invoke(this);
        }

        if (_current != null && _current.IsExecuting)
            return true;

        if (IsPaused)
        {
            return true;
        }

        if (!Update())
        {
            OnDone?.Invoke(this);
            Done();

            Stop();
            return false;
        }

        return true;
    }


    public void Pause()
    {
        if (!IsExecuting || IsPaused) return;
        IsPaused = true;

        Paused();
        OnPaused?.Invoke(this);
    }

    public void Resume()
    {
        IsPaused = false;
        Resumed();
    }
    

    public void Stop()
    {
        if (!IsExecuting) return;
        
        if (_routine is Coroutine routine)
            Parent.StopCoroutine(routine);
        if(_current != null && _current != this)
            _current.Stop();

        (this as IEnumerator).Reset();
        IsStopped = true;
        
        Stopped();
        OnStopped?.Invoke(this);
    }
    

    public Instruction Execute()
    {
        if (_current != null)
        {
            Debug.LogWarning($"Instruction { GetType().Name} is currently waiting for {_current.GetType().Name} and can't be stared right now.");
            return this;
        }

        if (!IsExecuting)
        {
            if (Started())
            {
                OnStarted?.Invoke(this);
                
                IsExecuting = true;
                IsStopped = false;
                _routine = Parent.StartCoroutine(this);
                
            }
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
            if (Started())
            {
                OnStarted?.Invoke(this);
                
                IsExecuting = true;
                IsStopped = false;
                _routine = (Parent = parent).StartCoroutine(this);
                
            }
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
        Stop();
        
        OnStarted = null;
        OnPaused = null;
        OnStopped = null;
        OnDone = null;
    }

    protected virtual bool Started() { return true; }

    protected virtual void Paused() { }

    protected virtual void Resumed() { }

    protected virtual void Stopped() { }

    protected virtual void Done() { }
    protected abstract bool Update();
}
