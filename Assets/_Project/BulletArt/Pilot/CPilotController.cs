using UnityEngine;
using UnityEngine.InputSystem;
using context = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Project.BulletArt
{
    public class CPilotController : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("키 설정")]
        [SerializeField] private InputActionReference _inputMove;
        [SerializeField] private InputActionReference _inputLowSpeed;
        [SerializeField] private InputActionReference _inputAttack;
        [SerializeField] private InputActionReference _inputBomb;

        [Header("이동 설정")] // 초당
        [SerializeField] private float _fastSpeed = 10f;
        [SerializeField] private float _slowSpeed = 5f;

        // 기능 합성
        [SerializeField] private PilotMover _pilotMover = new();
        [SerializeField] private PilotAttack _pilotAttack = new();
        [SerializeField] private PilotBomb _pilotBomb = new();

        // 입력값 캐싱
        private Vector2 _move = Vector2.zero;
        private bool _isLowSpeed = false;
        private bool _isAttack = false;
        #endregion

        #region ─────────────────────────▷ 핸들 메서드 ◁─────────────────────────
        private void HandleMovePerformed(context ctx)
        {
            Vector2 move = ctx.ReadValue<Vector2>();
            _move = Vector2.ClampMagnitude(move, 1f);
        }
        private void HandleMoveCanceled(context ctx)
        {
            _move = Vector2.zero;
        }
        private void HandleLowSpeedPerformed(context ctx)
        {
            _isLowSpeed = true;
        }
        private void HandleLowSpeedCanceled(context ctx)
        {
            _isLowSpeed = false;
        }
        private void HandleAttackPerformed(context ctx)
        {
            _isAttack = true;
        }
        private void HandleAttackCanceled(context ctx)
        {
            _isAttack = false;
        }
        private void HandleBombPerformed(context ctx)
        {
            _pilotBomb.Execute();
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Update()
        {
            if(_move != Vector2.zero) {
                _pilotMover.Execute(_move, _isLowSpeed);
            }
            if(_isAttack) {
                _pilotAttack.Execute();
            }
        }
        private void Awake() // 바인드
        {
            if(_inputMove != null) {
                _inputMove.action.performed += HandleMovePerformed;
                _inputMove.action.canceled += HandleMoveCanceled;
            }
            if (_inputLowSpeed != null) {
                _inputLowSpeed.action.performed += HandleLowSpeedPerformed;
                _inputLowSpeed.action.canceled += HandleLowSpeedCanceled;
            }
            if (_inputAttack != null) {
                _inputAttack.action.performed += HandleAttackPerformed;
                _inputAttack.action.canceled += HandleAttackCanceled;
            }
            if (_inputBomb != null) {
                _inputBomb.action.performed += HandleBombPerformed;
            }
        }

        private void OnDestroy() // 언바인드
        {
            if (_inputMove != null) {
                _inputMove.action.performed -= HandleMovePerformed;
                _inputMove.action.canceled -= HandleMoveCanceled;
            }
            if (_inputLowSpeed != null) {
                _inputLowSpeed.action.performed -= HandleLowSpeedPerformed;
                _inputLowSpeed.action.canceled -= HandleLowSpeedCanceled;
            }
            if (_inputAttack != null) {
                _inputAttack.action.performed -= HandleAttackPerformed;
                _inputAttack.action.canceled -= HandleAttackCanceled;
            }
            if (_inputBomb != null) {
                _inputBomb.action.performed -= HandleBombPerformed;
            }
        }

        private void OnEnable() // 구독
        {
            if (_inputMove != null) {
                _inputMove.action.Enable();
            }
            if (_inputLowSpeed != null) {
                _inputLowSpeed.action.Enable();
            }
            if (_inputAttack != null) {
                _inputAttack.action.Enable();
            }
            if (_inputBomb != null) {
                _inputBomb.action.Enable();
            }
        }

        private void OnDisable() // 구독 해제
        {
            if (_inputMove != null) {
                _inputMove.action.Disable();
            }
            if (_inputLowSpeed != null) {
                _inputLowSpeed.action.Disable();
            }
            if (_inputAttack != null) {
                _inputAttack.action.Disable();
            }
            if (_inputBomb != null) {
                _inputBomb.action.Disable();
            }
        }
        #endregion
    }
}
