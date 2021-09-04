using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Instructions;
using UnityEngine;

public class OreWork : Instruction
{
    private int _oreCount;
    private Transform _oreTransform;
    private LayerMask _layer;
    private readonly Instruction[] _instructions;
    private int _currentInstructionIndex=-1;

    public OreWork(MonoBehaviour actor,float produceSpeed) : base(actor)
    {
        _layer = LayerMask.NameToLayer("Ore");
        _instructions = new Instruction[]
        {
            new CheckCircleCast(actor, _layer, 1),
            new FillProgressBar(actor,produceSpeed),
            new MoveToTarget(actor, actor.transform, 0.5f, 0.5f )
        };
        
        var checkCircleCast = _instructions.FirstOrDefault(x => x is CheckCircleCast);
        checkCircleCast.OnDone += instruction => GetHit(instruction as IInstruction<Transform>);
        
        var moveToPoint = _instructions.FirstOrDefault(x => x is MoveToTarget);
        moveToPoint.OnStarted += instruction => SetOreToMove(instruction as IInstruction<Transform>);
        moveToPoint.OnDone += instruction => Object.Destroy(_oreTransform.gameObject);
    }

    private void GetHit(IInstruction<Transform> instruction)
    {
        _oreTransform = instruction.Value;
    }

    private void SetOreToMove(IInstruction<Transform> instruction)
    {
        instruction.Value = _oreTransform;
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