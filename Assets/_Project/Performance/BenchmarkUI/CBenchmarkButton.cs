using UnityEngine;
using Project.Default;
using TMPro;
using UnityEngine.UI;
using System.Reflection;

namespace Project.Performance
{
    /// <summary>
    /// 빌드 환경에서 UTimer의 벤치마크 결과를 UI에 표시합니다.
    /// </summary>
    public class CBenchmarkButton : AMono
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("참조 연결")]
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        private bool _isInitialize = false;
        private string _name;
        #endregion

        #region ─────────────────────────▷ 공개 멤버 ◁─────────────────────────
        public Button GetButton => _button;
        public void Initialize(AMono target, MethodInfo method,
            TextMeshProUGUI logText, string name)
        {
            if (_isInitialize)
            {
                UDebug.Print("초기화 함수가 중복으로 호출되었습니다.", LogType.Warning, this);
                return;
            }

            _isInitialize = true;
            SetName(name);
            AddClick(target, method, logText);
        }
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        // 버튼 이름 설정
        private void SetName(string name)
        {
            if (name.IsBlank())
            {
                UDebug.Print("벤치마크 버튼 프리팹에 빈 문자열이 전달되었습니다.", LogType.Error, this);
                return;
            }

            _text.text = name;
        }
        // 버튼 클릭 이벤트 연결
        private void AddClick(AMono target, MethodInfo method, TextMeshProUGUI logText)
        {
            _button.onClick.AddListener(() =>
            {
                PrintTitle(logText);
                method.Invoke(target, null);
            });
        }
        // 테스트 시 타이틀 문자열 출력
        private void PrintTitle(TextMeshProUGUI logText)
        {
            if (logText == null) return;
            if (_name == null) return;

            logText.text += $"\n\n※{_name} 테스트를 시작합니다.";
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Awake()
        {
            if (_button == null || _text == null)
            {
                UDebug.Print("필수 컴포넌트가 연결되지 않았습니다.", LogType.Error, this);
            }
        }
        #endregion
    }
}
