using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.Rename
{
    /// <summary>
    /// 선택된 에셋들의 이름을 일괄 변경합니다.
    /// </summary>
    public class RenameAsset : EditorWindow
    {
        #region ─────────────────────────▷ 설정 값 ◁─────────────────────────
        // 치환 옵션
        private string _find = "";
        private string _replace = "";
        private bool _caseSensitive = true;

        // 접두사 / 접미사
        private string _prefix = "";
        private string _suffix = "";

        // 변환 옵션
        private ESeparatorMode _separatorMode = ESeparatorMode.None;
        private ECapitalMode _capitalMode = ECapitalMode.None;
        #endregion

        #region ─────────────────────────▷ 윈도우 ◁─────────────────────────
        [MenuItem("Tools/이름 변경")]
        public static void ShowWindow()
        {
            RenameAsset window = GetWindow<RenameAsset>("이름 일괄 변경");
            window.minSize = new Vector2(320f, 320f);
            window.maxSize = new Vector2(320f, 320f);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("문자열 치환", EditorStyles.boldLabel);
            _find = EditorGUILayout.TextField("찾을 단어", _find);
            _replace = EditorGUILayout.TextField("바꿀 단어", _replace);
            _caseSensitive = EditorGUILayout.Toggle("대소문자 구분", _caseSensitive);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("접두사 / 접미사", EditorStyles.boldLabel);
            _prefix = EditorGUILayout.TextField("접두사", _prefix);
            _suffix = EditorGUILayout.TextField("접미사", _suffix);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("서식 변환 옵션", EditorStyles.boldLabel);
            _separatorMode = (ESeparatorMode)EditorGUILayout.EnumPopup("구분자 변환", _separatorMode);
            _capitalMode = (ECapitalMode)EditorGUILayout.EnumPopup("단어 첫 문자", _capitalMode);
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
                Debug.LogWarning("[Rename] 선택한 에셋이 없습니다.");
                return;
            }

            int successCount = 0;
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
                string newName = BuildNewName(oldName);

                // 변경점이 없으면 건너뜀
                if (oldName == newName)
                {
                    continue;
                }

                string errorMsg = AssetDatabase.RenameAsset(path, newName);
                if (string.IsNullOrEmpty(errorMsg))
                {
                    ++successCount;
                }
                else
                {
                    Debug.LogError($"[Rename] {oldName}의 이름을 변경하지 못했습니다.\n{errorMsg}", obj);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[Rename] 총 {successCount}개의 에셋 이름을 변경했습니다.");
        }
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        // 새 이름 작성
        private string BuildNewName(string original)
        {
            string current = original;

            // 치환
            if (!string.IsNullOrEmpty(_find))
            {
                if (_caseSensitive)
                {
                    current = current.Replace(_find, _replace);
                }
                else
                {
                    // 대소문자 무시 치환
                    StringBuilder replaceSb = new StringBuilder();
                    int cursor = 0;
                    while (cursor < current.Length)
                    {
                        int found = current.IndexOf(_find, cursor, StringComparison.OrdinalIgnoreCase);
                        if (found < 0)
                        {
                            replaceSb.Append(current, cursor, current.Length - cursor);
                            break;
                        }
                        replaceSb.Append(current, cursor, found - cursor);
                        replaceSb.Append(_replace);
                        cursor = found + _find.Length;
                    }
                    current = replaceSb.ToString();
                }
            }

            // 포맷팅
            if (_separatorMode != ESeparatorMode.None || _capitalMode != ECapitalMode.None)
            {
                string[] words = current.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);

                if (_capitalMode != ECapitalMode.None)
                {
                    for (int i = 0; i < words.Length; ++i)
                    {
                        words[i] = ApplyCapital(words[i]);
                    }
                }

                string separator = _separatorMode == ESeparatorMode.SpaceToUnderscore ? "_" : " ";
                current = string.Join(separator, words);
            }

            // 접두사 / 접미사
            StringBuilder finalSb = new StringBuilder();
            if (!string.IsNullOrEmpty(_prefix) && !current.StartsWith(_prefix))
            {
                finalSb.Append(_prefix);
            }

            finalSb.Append(current);

            if (!string.IsNullOrEmpty(_suffix) && !current.EndsWith(_suffix))
            {
                finalSb.Append(_suffix);
            }

            return finalSb.ToString();
        }

        // 첫 문자 대소문자 적용
        private string ApplyCapital(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }

            char first = _capitalMode == ECapitalMode.UpperFirst
                ? char.ToUpper(word[0])
                : char.ToLower(word[0]);

            if (word.Length == 1)
            {
                return first.ToString();
            }
            return first + word.Substring(1);
        }
        #endregion

        #region ─────────────────────────▷ 중첩 타입 ◁─────────────────────────
        // 공백/언더바 구분자 변환 방식
        private enum ESeparatorMode
        {
            None = 0,
            SpaceToUnderscore,
            UnderscoreToSpace,
        }

        // 단어 첫 문자 대소문자 변환 방식
        private enum ECapitalMode
        {
            None = 0,
            UpperFirst,
            LowerFirst,
        }
        #endregion
    }
}
