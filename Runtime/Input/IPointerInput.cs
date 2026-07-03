using System;
using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 마우스/터치를 통합한 포인터 입력 인터페이스
    /// </summary>
    public interface IPointerInput
    {
        bool JustPressed { get; }
        bool JustReleased { get; }
        bool JustTapped { get; }
        bool JustFastTapped { get; }
        bool IsPressed { get; }
        bool IsDragging { get; }

        bool IsEnabled { get; set; }

        Vector2 GetScreenPos { get; }
        Vector2 GetScreenPosDelta { get; }
        Vector2 GetScreenDragDelta { get; }
        Vector2 GetWorldPos { get; }
        Vector2 GetWorldPosDelta { get; }
        Vector2 GetPressStartWorldPos { get; }
        float DragThresholdScreenRadius { get; set; }
        float FastTapThreshold { get; }

        event Action<Vector2> OnPress;
        event Action<Vector2> OnRelease;
        event Action<Vector2> OnTap;
        event Action<Vector2> OnFastTap;
        event Action<Vector2> OnDragStart;
        event Action<Vector2> OnDrag;
        event Action<Vector2> OnDragEnd;

        void Tick(float deltaTime);
    }
}
