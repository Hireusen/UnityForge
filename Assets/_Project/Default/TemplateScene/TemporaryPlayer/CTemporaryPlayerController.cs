using UnityEngine;

namespace Project.Default
{
    [RequireComponent(typeof(Rigidbody))]
    public class CTemporaryPlayerController : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("조작 설정")]
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _sensitivity = 10f;

        private Rigidbody _rb;
        private float _inputV;
        private float _inputH;
        private float _inputMouseX;
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private void GetInput()
        {
            _inputV = Input.GetAxisRaw(K.AXIS_VERTICAL);
            _inputH = Input.GetAxisRaw(K.AXIS_HORIZONTAL);
            _inputMouseX += Input.GetAxis(K.AXIS_MOUSE_X) * _sensitivity;
        }

        private void MovePlayer()
        {
            Vector3 moveDir = (transform.forward * _inputV + transform.right * _inputH).normalized;
            Vector3 targetVelocity = moveDir * _moveSpeed;
            targetVelocity.y = _rb.linearVelocity.y;

            _rb.linearVelocity = targetVelocity;
        }

        private void RotatePlayer()
        {
            if (_inputMouseX == 0f) return;

            Quaternion deltaRotation = Quaternion.Euler(0f, _inputMouseX, 0f);
            _rb.MoveRotation(_rb.rotation * deltaRotation);
            _inputMouseX = 0f;
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            GetInput();
        }

        private void FixedUpdate()
        {
            MovePlayer();
            RotatePlayer();
        }
        #endregion
    }
}
