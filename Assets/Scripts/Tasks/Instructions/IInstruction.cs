using System;
using UnityEngine;

public interface IInstruction<T>
{
    T Value { get; set; }
    bool IsExecuting { get; }
    bool IsPaused { get; }

    Instruction Execute(MonoBehaviour parent);
    void Pause();
    void Resume();
    void Stop();

    event Action<Instruction> OnStarted;
    event Action<Instruction> OnPaused;
    event Action<Instruction> OnStopped;
    event Action<Instruction> OnDone;
}