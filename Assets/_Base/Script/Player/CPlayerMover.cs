using UnityEngine;

/// <summary>
/// 플레이어 오브젝트가 키를 입력받아 이동할 수 있도록 합니다.
/// </summary>
public class CPlayerMover : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("키 입력")] // 이동은 GetAxis
    [SerializeField] private KeyCode _runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("디버그")]
    [SerializeField] private bool _log = false;
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Update()
    {
        
    }
    #endregion
}
