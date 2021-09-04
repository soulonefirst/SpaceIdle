using UnityEngine;

public sealed class MoveToTarget : Instruction, IInstruction<Transform>
{
    public Transform Value
    {
        get => _actor;
        set => _actor = value;
    }
    private Transform _actor;
    private readonly Transform _target;
    private readonly float _speed;
    private readonly float _threshold;

    public MoveToTarget(MonoBehaviour parent, Transform target, float speed, float threshold) : base(parent)
    {
        _target = target;
        _speed = speed;
        _threshold = threshold;
    }
    
    protected override bool Update()
    {
        var actorPosition = _actor.position;
        var targetPosition = _target.position;
        
        actorPosition = Vector3.Lerp(actorPosition, targetPosition, Time.deltaTime * _speed);
        _actor.position = actorPosition;

        return Vector3.Distance(actorPosition, targetPosition) >= _threshold;
    }


}