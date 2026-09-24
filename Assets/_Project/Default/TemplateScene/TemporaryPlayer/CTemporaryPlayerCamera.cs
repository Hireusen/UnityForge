using UnityEngine;

namespace Project.Default
{
    public class CTemporaryPlayerCamera : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("카메라 모드")]
        [SerializeField] private ECameraMode _cameraMode = ECameraMode.FirstPerson;

        [Header("참조 연결")]
        [SerializeField] private Transform _cameraTr;
        [SerializeField] private Transform _targetTr;

        [Header("1인칭 설정")]
        [SerializeField] private Vector3 _firstPersonCameraOffset = new Vector3(0f, 3f, 0.5f);

        [Header("3인칭 설정")]
        [SerializeField] private Vector3 _thirdPersonCameraOffset = new Vector3(0f, 6f, -6f);
        [SerializeField] private Vector3 _thirdPersonLookAtOffset = new Vector3(0f, 4f, 1f);
        [SerializeField] private float _thirdPersonSmoothSpeed = 12f;

        [Header("쿼터뷰 설정")]
        [SerializeField] private Vector3 _quarterViewCameraOffset = new Vector3(5f, 15f, -5f);
        [SerializeField] private Vector3 _quarterViewLookAtOffset = new Vector3(0f, 0f, 0f);
        [SerializeField] private float _quarterViewSmoothSpeed = 4f;
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private void SmoothLookAt(Vector3 targetPos, float smoothSpeed)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(targetPos - _cameraTr.position);
            _cameraTr.rotation = Quaternion.Slerp(_cameraTr.rotation, desiredRotation, Time.deltaTime * smoothSpeed);
        }

        private void SmoothPositionTo(Vector3 desiredPos, float smoothSpeed)
        {
            _cameraTr.position = Vector3.Slerp(_cameraTr.position, desiredPos, Time.deltaTime * smoothSpeed);
        }

        // 타겟의 정면 기준으로 오프셋 적용
        private Vector3 GetOffsetPosition(Vector3 position, Vector3 offset)
        {
            position += _targetTr.forward * offset.z;
            position += _targetTr.right * offset.x;
            position.y += offset.y;
            return position;
        }

        private void SetFirstPerson()
        {
            _cameraTr.position = GetOffsetPosition(_targetTr.position, _firstPersonCameraOffset); ;
            _cameraTr.rotation = transform.rotation;
        }

        private void SetThirdPerson()
        {
            Vector3 desiredPos = GetOffsetPosition(_targetTr.position, _thirdPersonCameraOffset);
            SmoothPositionTo(desiredPos, _thirdPersonSmoothSpeed);
            Vector3 targetPos = GetOffsetPosition(_targetTr.position, _thirdPersonLookAtOffset);
            SmoothLookAt(targetPos, _thirdPersonSmoothSpeed);
        }

        private void SetQuarterView()
        {
            Vector3 targetPos = _targetTr.position;
            SmoothPositionTo(targetPos + _quarterViewCameraOffset, _quarterViewSmoothSpeed);
            SmoothLookAt(targetPos + _quarterViewLookAtOffset, _quarterViewSmoothSpeed);
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Awake()
        {
            if(_cameraTr == null)
            {
                _cameraTr = Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (_cameraTr == null) return;
            if (_targetTr == null) return;

            switch(_cameraMode)
            {
                case ECameraMode.FirstPerson:
                    SetFirstPerson();
                    break;
                case ECameraMode.ThirdPerson:
                    SetThirdPerson();
                    break;
                case ECameraMode.QuarterView:
                    SetQuarterView();
                    break;
                default:
                    UDebug.Print($"알 수 없는 카메라 모드 : {_cameraMode}");
                    break;
            }
        }
        #endregion

        #region ─────────────────────────▷ 중첩 타입 ◁─────────────────────────
        public enum ECameraMode
        {
            FirstPerson,
            ThirdPerson,
            QuarterView
        }
        #endregion
    }
}
