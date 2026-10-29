using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.UniTaskLabo
{
    public class CSinForwardMover : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("조작 설정")]
        [SerializeField] private KeyCode _key = KeyCode.Q;
        [SerializeField] private Rigidbody _target;

        [Header("속도 설정")]
        [SerializeField] private float _dashDistance = 1.5f;
        [SerializeField] private float _moveTime = 0.4f;

        private CancellationTokenSource _cts;
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private async UniTaskVoid ForwardMoveSeqeunce(CancellationToken ct)
        {
            try
            {
                float startTime = Time.time;
                float endTime = startTime + _moveTime;

                Vector3 startPos = _target.position;
                Vector3 targetPos = startPos + (_target.transform.forward * _dashDistance);

                while (Time.time < endTime)
                {
                    float t = (Time.time - startTime) / _moveTime;
                    float easeOut = 1f - (1f - t) * (1f - t);
                    Vector3 movement = Vector3.Lerp(startPos, targetPos, easeOut);
                    _target.MovePosition(movement);
                    await UniTask.Yield(PlayerLoopTiming.FixedUpdate, ct);
                }
            }
            finally
            {
                if (_cts != null && _cts.Token == ct)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    _cts = null;
                }
            }
        }

        private void TryDispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void OnDisable()
        {
            TryDispose();
        }
        private void Update()
        {
            if (Input.GetKeyDown(_key))
            {
                TryDispose();
                _cts = new CancellationTokenSource();
                var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, destroyCancellationToken).Token;
                ForwardMoveSeqeunce(linkedToken).Forget();
            }
        }
        #endregion
    }
}
