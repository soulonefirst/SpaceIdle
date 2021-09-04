using UnityEngine;

namespace Instructions
{
    public class FillProgressBar : Instruction
    {
        private readonly GameObject _progressBar;
        private readonly Transform _barTrans;
        private readonly float _fillSpeed;
        private readonly Wait _wait;

        public FillProgressBar(MonoBehaviour parent, float fillSpeed) : base(parent)
        {
            _fillSpeed = fillSpeed;
            _progressBar = parent.transform.GetChild(0).gameObject;
            _barTrans = _progressBar.transform.Find("Bar").transform;
            _wait = new Wait(0.1f,parent);
        }
        private void ResetBar()
        {
            _barTrans.localScale = Vector3.zero;    
        }

        protected override bool Started()
        {
            _progressBar.SetActive(true);
            return true;
        }

        protected override bool Update()
        {
            var newScaleX = _barTrans.localScale.x + (1 / _fillSpeed / 10);
            _barTrans.localScale = new Vector3(newScaleX, 1, 1);
            
            _current = _wait.Execute();
            return (newScaleX <= 1.0f);
        }

        protected override void Done()
        {
            ResetBar();
            _progressBar.SetActive(false);
        }

        protected override void Stopped()
        {
            ResetBar();
            _progressBar.SetActive(false);
        }
    }
}