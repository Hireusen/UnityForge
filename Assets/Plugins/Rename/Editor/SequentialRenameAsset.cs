using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.Rename
{
    /// <summary>
    /// 선택된 에셋들의 이름을 공통 문자열과 순차적인 번호로 일괄 변경합니다.
    /// </summary>
    public class SequentialRenameAsset : EditorWindow
    {
        #region ─────────────────────────▷ 설정 값 ◁─────────────────────────
        // 기본 이름
        private string _baseName = "NewAsset";

        // 순차 번호 옵션
        private ESeparatorType _separatorType = ESeparatorType.Underscore;
        private int _startIndex = 1;
        private int _digitCount = 2;
        #endregion

        #region ─────────────────────────▷ 윈도우 ◁─────────────────────────
        [MenuItem("Tools/이름 변경/순차적")]
        public static void ShowWindow()
        {
            SequentialRenameAsset window = GetWindow<SequentialRenameAsset>("순차적 이름 변경");
            window.minSize = new Vector2(320f, 300f);
            window.maxSize = new Vector2(320f, 300f);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("이름 설정", EditorStyles.boldLabel);
            _baseName = EditorGUILayout.TextField("공통 문자열", _baseName);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("번호 부여 옵션", EditorStyles.boldLabel);
            _separatorType = (ESeparatorType)EditorGUILayout.EnumPopup("구분자", _separatorType);
            _startIndex = EditorGUILayout.IntField("시작 번호", _startIndex);

            // 자릿수 1~5 조절 슬라이더
            _digitCount = EditorGUILayout.IntSlider("숫자 자릿수(0 패딩)", _digitCount, 1, 5);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // 미리보기
            EditorGUILayout.LabelField("미리보기", EditorStyles.boldLabel);
            GUI.color = Color.cyan;
            EditorGUILayout.LabelField(BuildNewName(_startIndex));
            GUI.color = Color.white;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("선택한 에셋의 이름 변경", GUILayout.Height(30)))
            {
                ExecuteRename();
            }
        }
        #endregion

        #region ─────────────────────────▷ 실행 ◁─────────────────────────
        private void ExecuteRename()
        {
            // 프로젝트 창에서 선택된 에셋 추출
            Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

            if (selectedAssets.Length == 0)
            {
                Debug.LogWarning("[SequentialRename] 선택한 에셋이 없습니다.");
                return;
            }

            // 에셋 이름순으로 정렬
            Array.Sort(selectedAssets, (a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

            int successCount = 0;
            int currentIndex = _startIndex;
            int length = selectedAssets.Length;

            for (int i = 0; i < length; ++i)
            {
                Object obj = selectedAssets[i];
                string path = AssetDatabase.GetAssetPath(obj);

                // 폴더 제외
                if (AssetDatabase.IsValidFolder(path))
                {
                    continue;
                }

                string oldName = obj.name;
                string newName = BuildNewName(currentIndex);

                // 변경점이 없으면 건너뜀
                if (oldName == newName)
                {
                    currentIndex++;
                    continue;
                }

                string errorMsg = AssetDatabase.RenameAsset(path, newName);
                if (string.IsNullOrEmpty(errorMsg))
                {
                    ++successCount;
                    ++currentIndex; // 이름 변경에 성공했을 때만 인덱스 증가
                }
                else
                {
                    Debug.LogError($"[SequentialRename] {oldName}의 이름을 변경하지 못했습니다.\n{errorMsg}", obj);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[SequentialRename] 총 {successCount}개의 에셋 이름을 순차적으로 변경했습니다.");
        }
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        // 새 이름 작성
        private string BuildNewName(int index)
        {
            string separator = "";
            switch (_separatorType)
            {
                case ESeparatorType.Underscore: separator = "_"; break;
                case ESeparatorType.Dash: separator = "-"; break;
                case ESeparatorType.Dot: separator = "."; break;
                case ESeparatorType.Space: separator = " "; break;
                case ESeparatorType.None: separator = ""; break;
            }

            // 앞 0 채우기
            string numberString = index.ToString("D" + _digitCount);

            return $"{_baseName}{separator}{numberString}";
        }
        #endregion

        #region ─────────────────────────▷ 중첩 타입 ◁─────────────────────────
        private enum ESeparatorType
        {
            Underscore, // _
            Dash,       // -
            Dot,        // .
            Space,      // 공백
            None        // 없음
        }
        #endregion
    }
}
