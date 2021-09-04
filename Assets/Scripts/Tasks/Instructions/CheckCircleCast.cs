using System;
using UnityEngine;

namespace Instructions
{
    public class CheckCircleCast : Instruction, IInstruction<Transform>
    {
        public new Transform Value
        {
            get => _hitTransform;
            set { }
        }
        private Transform _hitTransform;
        private readonly LayerMask _layer;
        private readonly Transform _areaTransform;
        private readonly Wait _wait;

        public CheckCircleCast(MonoBehaviour parent, LayerMask layer, int castFrequency) : base(parent)
        {
            _areaTransform = parent.transform.GetChild(1).transform;
            _layer = layer;
            _wait = new Wait(castFrequency,parent);
        }

        protected override bool Update()
        {
            var hit = Physics2D.OverlapCircle(
                _areaTransform.parent.transform.position,
                _areaTransform.transform.localScale.x /2,
                1 << _layer);

            if (hit == false)
            {
              _current = _wait.Execute();
                return true;
            }
            
            _hitTransform = hit.transform;
            _hitTransform.gameObject.layer = 2;
            return false;

        }

    }
}