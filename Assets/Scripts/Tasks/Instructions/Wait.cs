using UnityEngine;

namespace Instructions
{
    public sealed class Wait : Instruction
    {
        private float _delay;
        private float _startTime;


        public Wait(MonoBehaviour parent) : base(parent) { }

        public Wait(float delay, MonoBehaviour parent) : base(parent)
        {
            _delay = delay;
        }

        public Instruction Execute(float delay)
        {
            _delay = delay;

            return base.Execute();
        }
        protected override bool Started()
        {
            _startTime = Time.time;
            return true;
        }
        protected override bool Update()
        {
            return Time.time - _startTime < _delay;
        }

        protected override void Paused()
        {
            _delay -= Time.time - _startTime;
        }

        protected override void Resumed()
        {
            _startTime = Time.time;
        }
    }
}