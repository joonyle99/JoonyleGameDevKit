using UnityEngine;

namespace JoonyleGameDevKit
{
    public class EffectAnimator : EffectBase, IAnimationEventHandler
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        protected override void OnPlay()
        {
            gameObject.SetActive(true);
            _animator.Rebind();
            _animator.Update(0f);
        }

        protected override void OnStop()
        {
            gameObject.SetActive(false);
        }

        public void OnAnimationEvent(string eventName)
        {
            if (eventName == "Complete") OnComplete();
        }
    }
}
