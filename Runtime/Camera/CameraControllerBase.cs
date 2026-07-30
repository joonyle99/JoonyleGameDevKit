using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 카메라 크기 계산 등 공통 기능만 제공하는 카메라 컨트롤러 베이스.
    /// 추적/경계 처리 등 실제 카메라 이동 로직은 프로젝트마다 다르므로
    /// 프로젝트에서는 CameraController : CameraControllerBase 형태로 상속해서 LateTick을 구현한다.
    /// </summary>
    public abstract class CameraControllerBase : MonoBehaviour
    {
        private Camera _camera;
        public Camera Camera => _camera;

        public float CameraWidth => CameraHeight * CameraAspect;
        public float CameraHeight => _camera.orthographicSize * 2f;
        public float CameraAspect => (float)Screen.width / (float)Screen.height;

        public abstract void LateTick(float deltaTime);

        public virtual void Initialize()
        {
            _camera = GetComponent<Camera>();
        }
    }
}
