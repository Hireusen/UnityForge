using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.UniTaskLabo
{
    public class CFadeUniTask : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("참조 연결")]
        [SerializeField] private CanvasGroup _canvas;

        [Header("속도 설정")]
        [SerializeField] private float _fadeTime = 1.5f;

        [Header("페이드 키")]
        [SerializeField] private KeyCode _fadeOutKey = KeyCode.T;
        [SerializeField] private KeyCode _fadeInKey = KeyCode.Y;

        private CancellationTokenSource _cts; // 명령기 역할
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private void TaskCancel()
        {
            if (_cts == null) return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        private async UniTaskVoid TestAll()
        {
            await UniTask.WhenAll(TaskA(), TaskB(), TaskC());
            Debug.Log("모든 작업이 끝났습니다.");
        }

        private async UniTaskVoid TestAny()
        {
            await UniTask.WhenAny(TaskA(), TaskB(), TaskC());
            Debug.Log("가장 먼저 끝난 작업이 있습니다.");
        }

        private async UniTask TaskA()
        {
            await UniTask.WaitForSeconds(1f);
        }
        private async UniTask TaskB()
        {
            await UniTask.WaitForSeconds(2f);
        }
        private async UniTask TaskC()
        {
            await UniTask.WaitForSeconds(4f);
        }

        private async UniTaskVoid FadeStart(float startAlpha, float endAlpha)
        {
            if (_cts != null)
            {
                TaskCancel();
            }

            _cts = new CancellationTokenSource();
            CancellationToken destoryToken = this.GetCancellationTokenOnDestroy();
            CancellationToken linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, destoryToken).Token;

            UniTask task = FadeRoutineAsync(startAlpha, endAlpha, _fadeTime, linkedToken);
            while(task.Status == UniTaskStatus.Pending)
            {
                await UniTask.Yield();
            }

            if (task.Status == UniTaskStatus.Succeeded)
            {
                Debug.Log("페이드가 정상 완료되었습니다.");
            }
            else if (task.Status == UniTaskStatus.Canceled)
            {
                Debug.Log("페이드가 취소되었습니다.");
            }
            else if (task.Status == UniTaskStatus.Pending)
            {
                Debug.Log("페이드가 진행중입니다.");
            }
            else
            {
                Debug.Log("무슨 일이 발생했지?");
            }
        }

        private async UniTask FadeRoutineAsync(float startAlpha, float endAlpha, float fadeTime, CancellationToken token)
        {
            try
            {
                float time = 0f;
                // 매 프레임
                while (time < fadeTime)
                {
                    float ratio = Mathf.Lerp(startAlpha, endAlpha, time / fadeTime);
                    time += Time.deltaTime;
                    _canvas.alpha = ratio;
                    await UniTask.Yield(token);
                }
                _canvas.alpha = endAlpha;
            }
            catch (OperationCanceledException) // 정상 취소
            {
                // Debug.Log("페이드가 중복 키 입력으로 취소되었습니다.");
            }
            finally
            {
                if (_cts != null && _cts.Token == token)
                {
                    TaskCancel();
                }
            }
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Update()
        {
            float startAlpha = _canvas.alpha;
            if (Input.GetKeyDown(_fadeOutKey))
            {
                Debug.Log("작업 시작 Any");
                //TestAny().Forget();
                FadeStart(startAlpha, 0f).Forget();
            }
            else if (Input.GetKeyDown(_fadeInKey))
            {
                Debug.Log("작업 시작 All");
                //TestAll().Forget();
                FadeStart(startAlpha, 1f).Forget();
            }
        }

        private void OnDestroy()
        {
            TaskCancel();
        }
        #endregion
    }
}
