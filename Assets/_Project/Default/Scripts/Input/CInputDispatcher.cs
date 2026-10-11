using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Default
{
    public class CInputDispatcher : ASingleton<CInputDispatcher>
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [SerializeField] private InputActionReference _move;
        [SerializeField] private InputActionReference _jump;
        [SerializeField] private InputActionReference _pause;

        private bool _isReady = false; // 바인딩을 한 번만 하게 막아주는 방어 변수
        #endregion

        #region ─────────────────────────▷ 공개 멤버 ◁─────────────────────────
        public event Action<Vector2> OnMove;
        public event Action OnJump;
        public event Action OnPause;
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        protected override void Initialize()
        {
            TryBind();
        }

        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            OnMove?.Invoke(ctx.ReadValue<Vector2>());
        }
        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            OnMove?.Invoke(Vector2.zero);
        }
        private void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
            OnJump?.Invoke();
        }
        private void OnPausePerformed(InputAction.CallbackContext ctx)
        {
            OnPause?.Invoke();
        }

        private bool TryBind()
        {
            if (_isReady) return false;
            if (_move == null || _move.action == null) return false;
            if (_jump == null || _jump.action == null) return false;
            if (_pause == null || _pause.action == null) return false;

            _move.action.performed += OnMovePerformed;
            _move.action.canceled += OnMoveCanceled;
            _jump.action.performed += OnJumpPerformed;
            _pause.action.performed += OnPausePerformed;

            _isReady = true;
            return true;
        }

        private bool TryUnbind()
        {
            if (!_isReady) return false;

            _move.action.performed -= OnMovePerformed;
            _move.action.canceled -= OnMoveCanceled;
            _jump.action.performed -= OnJumpPerformed;
            _pause.action.performed -= OnPausePerformed;

            _isReady = false;
            return true;
        }

        // 입력을 받을것인가?
        // 퍼즈 상태일 때 플레이어는 멈추고 UI는 유지같은 설계가 가능하도록
        private void EnableActions(bool enable)
        {
            if (!_isReady) return;

            if (enable)
            {
                _move.action.Enable();
                _jump.action.Enable();
                _pause.action.Enable();
            }
            else
            {
                _move.action.Disable();
                _jump.action.Disable();
                _pause.action.Disable();
            }
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        protected override void OnDestroy()
        {
            base.OnDestroy();
            TryUnbind();
        }
        private void OnEnable()
        {
            TryBind();
            EnableActions(true);
        }
        private void OnDisable()
        {
            EnableActions(false);
        }
        #endregion
    }
}
