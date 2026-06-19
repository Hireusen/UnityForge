using UnityEngine;

/// <summary>
/// Frameable을 상속받는 테스트용 컴포넌트
/// </summary>
public class TestFrameable : AFrameable, IUpdateFrameable
{
    // 실행 우선순위 정의
    public EUpdatePriority UpdatePriority => EUpdatePriority.Lv1;

    // 프레임 매니저에게 호출당할 함수
    public void ExecuteUpdateFrame()
    {
        
    }
}
