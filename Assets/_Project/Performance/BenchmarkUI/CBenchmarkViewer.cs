using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Project.Default; // UTimer 접근용

namespace Project.Performance
{
    /// <summary>
    /// UTimer의 벤치마크 결과를 화면에 표시합니다.
    /// </summary>
    public class CBenchmarkUIViewer : AMono
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("로그 출력")]
        [SerializeField] private TextMeshProUGUI _logText;

        [Header("자동 버튼 생성 설정")]
        [SerializeField] private RectTransform _buttonContainer;
        [SerializeField] private CBenchmarkButton _buttonPrefab;

        [Header("테스트 대상 컴포넌트 목록")]
        [SerializeField] private AMono[] _testTargets;
        #endregion

        #region ─────────────────────────▷ 유니티 콜백 ◁─────────────────────────
        private void Awake()
        {
            GenerateButtons();
        }

        private void OnEnable()
        {
            UTimer.OnBenchmarkComplete += AddLogBox;
        }

        private void OnDisable()
        {
            UTimer.OnBenchmarkComplete -= AddLogBox;
        }
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private void GenerateButtons()
        {
            // 초기 방어
            if (_buttonPrefab == null || _buttonContainer == null)
            {
                UDebug.Print("버튼 프리팹이나 컨테이너가 할당되지 않았습니다.", LogType.Error, this);
                return;
            }

            // 할당된 모든 테스트 컴포넌트를 순회합니다.
            foreach (AMono target in _testTargets)
            {
                if (target == null) continue;

                // 리플렉션
                MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in methods)
                {
                    // ContextMenu 어트리뷰트 추출
                    ContextMenu contextMenu = method.GetCustomAttribute<ContextMenu>();
                    if (contextMenu != null)
                    {
                        CreateButton(target, method, contextMenu.menuItem);
                    }
                }
            }
        }
        private void CreateButton(AMono target, MethodInfo method, string menuName)
        {
            // 버튼 생성
            CBenchmarkButton button = Instantiate(_buttonPrefab, _buttonContainer);
            Button component = button.GetButton;

            // 버튼 초기화
            button.Initialize(target, method, _logText, menuName);
        }

        private void AddLogBox(string msg)
        {
            if (_logText == null)
            {
                UDebug.Print("프리팹에 로그 박스가 없습니다.", LogType.Error, this);
                return;
            }

            _logText.text += $"\n{msg}";
        }
        #endregion

        #region ─────────────────────────▷ 공개 멤버 ◁─────────────────────────
        public void ClearLogBox()
        {
            if (_logText == null)
            {
                UDebug.Print("프리팹에 로그 박스가 없습니다.", LogType.Error, this);
                return;
            }

            _logText.text = "";
        }
        #endregion
    }
}
