using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JoonyleGameDevKit
{
    public class UISelectableFuncFeedback : UISelectableFeedback
    {
        private Action<bool> _action;

        public override void OnHoverEnter(PointerEventData eventData)
        {

        }

        public override void OnHoverExit(PointerEventData eventData)
        {

        }

        public override void OnPress(PointerEventData eventData)
        {
            _action?.Invoke(true);
        }

        public override void OnRelease(PointerEventData eventData)
        {
            _action?.Invoke(false);
        }

        public void SetFunc(Action<bool> action)
        {
            _action = action;
        }
    }
}
