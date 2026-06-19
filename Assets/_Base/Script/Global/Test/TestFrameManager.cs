using UnityEngine;

/// <summary>
/// 프레임 매니저의 안정성을 테스트하는 스크립트
/// </summary>
public class TestFrameManager : AFrameable, IUpdateFrameable
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("테스트 설정")]
    [SerializeField] private int _count = 500; // 샘플 생성 개수
    [SerializeField] private bool _start = false; // 샘플 생성 및 시작
    [SerializeField] private bool _clear = false; // 모든 샘플 청소
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────
    // 실행 우선순위 정의
    public EUpdatePriority UpdatePriority => EUpdatePriority.Lv5;

    // 프레임 매니저에게 호출당할 함수
    public void ExecuteUpdateFrame()
    {

    }
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void CreateFrameableObjects()
    {
        for (int i = 0; i < _count; ++i) {
            UObject.Create("FrameableObject").AddComponent<TestFrameable>();
        }
    }

    private void ClearFrameableObjects()
    {

    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────

    #endregion
}
