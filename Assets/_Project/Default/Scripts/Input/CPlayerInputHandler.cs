using UnityEngine;
using System.Collections;

namespace Project.Default
{
    public class CPlayerInputHandler : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("이동 설정")]
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _rotateSharpness = 15f;
        [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _groundStick = -2f;

        [Header("참조 연결")]
        [SerializeField] private CharacterController _cc;
        [SerializeField] private Transform _cameraTr;

        private Vector2 _moveInput; // 디스패처가 쏘는 Move(Vector2)는 순간이기 때문에 지속성을 가지기 위해 저장
        private bool _jumpRequested;
        private float _verticalVelocity; // 리지드바디를 사용하지 않고 있기 때문에 직접 y속도 누적용
        private bool _isBind;
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private void HandleMove(Vector2 moved)
        {
            _moveInput = Vector2.ClampMagnitude(moved, 1f);
        }

        private void HandleJump()
        {
            _jumpRequested = true;
        }

        private bool TryBindDispatcher()
        {
            if (_isBind) return false;
            var dispatcher = CInputDispatcher.Ins;
            if (dispatcher == null) return false;

            dispatcher.OnMove += HandleMove;
            dispatcher.OnJump += HandleJump;
            _isBind = true;
            return true;
        }

        private bool TryUnbindDispatcher()
        {
            if (!_isBind) return false;
            var dispatcher = CInputDispatcher.Ins;
            if (dispatcher == null) return false;

            dispatcher.OnMove -= HandleMove;
            dispatcher.OnJump -= HandleJump;
            _isBind = false;
            return true;
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Awake()
        {
            UDebug.IsNull(_cc);
            if(_cameraTr == null && Camera.main != null)
            {
                _cameraTr = Camera.main.transform;
            }
        }
        private void Start()
        {
            TryBindDispatcher();
        }
        private void OnEnable()
        {
            TryBindDispatcher();
        }
        private void OnDisable()
        {
            TryUnbindDispatcher();
        }
        #endregion
    }
}
