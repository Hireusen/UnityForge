using UnityEngine;

namespace Project.BulletArt
{
    [System.Serializable]
    public class PilotMover
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("파일럿 이동")]
        [SerializeField] private Transform _pilotTr;
        [SerializeField] private PilotMoveSO _move;

        [Header("화면 경계")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector2 _padding = new Vector2(0.5f, 0.5f);
        #endregion

        #region ─────────────────────────▷ 공개 멤버 ◁─────────────────────────
        public void Execute(Vector2 input, bool isLowSpeed)
        {
            if (_pilotTr == null || _move == null || _camera == null) return;

            Vector2 movement = input * (isLowSpeed ? _move.LowSpeed : _move.HighSpeed);
            Vector3 targetPos = _pilotTr.position + (Vector3)(movement * Time.deltaTime);

            // 화면 경계 계산
            float camHeight = _camera.orthographicSize;
            float camWidth = camHeight * _camera.aspect;
            Vector3 camPos = _camera.transform.position;
            Vector2 min = new Vector2(camPos.x - camWidth + _padding.x, camPos.y - camHeight + _padding.y);
            Vector2 max = new Vector2(camPos.x + camWidth - _padding.x, camPos.y + camHeight - _padding.y);

            // 제한 및 적용
            targetPos.x = Mathf.Clamp(targetPos.x, min.x, max.x);
            targetPos.y = Mathf.Clamp(targetPos.y, min.y, max.y);
            _pilotTr.position = targetPos;
        }
        #endregion
    }
}
