using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace JoonyleGameDevKit
{
    public class PanelToggler : MonoBehaviour
    {
        [SerializeField] private RectTransform _panel;

        [Space]

        [SerializeField] private float _expandedX; // 패널이 보일 때 X
        [SerializeField] private float _collapsedX; // 화면 우측 밖 X
        [SerializeField] private float _duration;

        [Space]
        
        [SerializeField] private Ease _openEase = Ease.OutExpo;
        [SerializeField] private Ease _closeEase = Ease.InExpo;

        [Space]

        [SerializeField] private bool _isExpanded;
        public bool IsExpanded => _isExpanded;

        public bool IsExpandedDirty { get; set; }

        [Space]

        public UnityEvent OnOpened;
        public UnityEvent OnClosed;

        private void Start()
        {
            var panelPos = _panel.anchoredPosition;
            panelPos.x = _isExpanded ? _expandedX : _collapsedX;
            _panel.anchoredPosition = panelPos;
        }

        public void TogglePanel()
        {
            if (_isExpanded) Close();
            else Open();
        }

        public void Open()
        {
            if (_isExpanded) return;
            _isExpanded = true;
            _panel.DOAnchorPosX(_expandedX, _duration).SetEase(_openEase).SetUpdate(true);
            OnOpened?.Invoke();
        }

        public void Close()
        {
            if (!_isExpanded) return;
            _isExpanded = false;
            _panel.DOAnchorPosX(_collapsedX, _duration).SetEase(_closeEase).SetUpdate(true);
            OnClosed?.Invoke();
        }
    }
}
