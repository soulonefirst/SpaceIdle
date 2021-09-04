using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Instructions;
using UnityEngine;

public sealed class CreateOre : Instruction
{
    private readonly Instruction[] _instructions;
    private int _currentInstructionIndex=-1;

    public CreateOre(MonoBehaviour actor, float produceSpeed) : base(actor)
    {
        _instructions = new Instruction[]
        {
            new FillProgressBar(actor, produceSpeed),
            new CreatePrefab(actor, "OrePiece", new Vector3(Random.Range(-2.0f,2.0f),Random.Range(-2.0f,2.0f),0))
        };

        var createPrefab = _instructions.FirstOrDefault(x => x is CreatePrefab);
        // ReSharper disable once PossibleNullReferenceException
        createPrefab.OnStarted += instruction => NewPosition(instruction as IInstruction<Vector3>);
        
    }
    private void NewPosition(IInstruction<Vector3> obj)
    {
        obj.Value = new Vector3(Random.Range(-2.0f,2.0f),Random.Range(-2.0f,2.0f),0);
    }

    protected override bool Update()
    {
        _currentInstructionIndex++;
        if (_currentInstructionIndex < _instructions.Length)
        {
            _current = _instructions[_currentInstructionIndex].Execute();
        }
        else
            _currentInstructionIndex = -1;

        return true;
    }

    protected override void Stopped()
    {
        _currentInstructionIndex = -1;
    }
}