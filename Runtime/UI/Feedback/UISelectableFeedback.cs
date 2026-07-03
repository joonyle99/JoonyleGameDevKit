using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JoonyleGameDevKit
{
    public abstract class UISelectableFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        protected Selectable selectable;
        public Selectable Selectable
        {
            get
            {
                if (selectable == null)
                {
                    selectable = GetComponent<Selectable>();
                }

                return selectable;
            }
        }

        public bool Interactable { get; set; } = true;

        public bool HasSelectable => Selectable != null;

        protected virtual void Awake()
        {

        }
        protected virtual void Start()
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable)
            {
                return;
            }

            if (HasSelectable && !Selectable.interactable)
            {
                return;
            }

            OnHoverEnter(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!Interactable)
            {
                return;
            }

            if (HasSelectable && !Selectable.interactable)
            {
                return;
            }

            OnHoverExit(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable)
            {
                return;
            }

            if (HasSelectable && !Selectable.interactable)
            {
                return;
            }

            OnPress(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!Interactable)
            {
                return;
            }

            if (HasSelectable && !Selectable.interactable)
            {
                return;
            }

            OnRelease(eventData);
        }

        public abstract void OnHoverEnter(PointerEventData eventData);
        public abstract void OnHoverExit(PointerEventData eventData);
        public abstract void OnPress(PointerEventData eventData);
        public abstract void OnRelease(PointerEventData eventData);
    }
}
