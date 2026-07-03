using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JoonyleGameDevKit
{
    [RequireComponent(typeof(Image))]
    public class UISelectableColorFeedback : UISelectableFeedback
    {
        [SerializeField] private Color _targetColor = new Color(0xD7 / 255f, 0xD7 / 255f, 0xD7 / 255f, 0xFF / 255f);
        private Color _originalColor;

        private Image _image;

        protected override void Awake()
        {
            base.Awake();

            _image = GetComponent<Image>();
        }
        protected override void Start()
        {
            base.Start();

            _originalColor = _image.color;
        }

        public override void OnHoverEnter(PointerEventData eventData)
        {

        }

        public override void OnHoverExit(PointerEventData eventData)
        {

        }

        public override void OnPress(PointerEventData eventData)
        {
            _image.color = _targetColor;
        }
        public override void OnRelease(PointerEventData eventData)
        {
            _image.color = _originalColor;
        }
    }
}
