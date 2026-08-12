using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Plugins.MaterialTools
{
    /// <summary>
    /// 선택된 머티리얼과 텍스처 중 이름이 동일한 에셋들을 자동으로 연결합니다.
    /// </summary>
    public class AutoAssignTexture : EditorWindow
    {
        #region ─────────────────────────▷ 설정 값 ◁─────────────────────────
        private EPropertyName _propertyMode = EPropertyName.URP;
        #endregion

        #region ─────────────────────────▷ 윈도우 ◁─────────────────────────
        [MenuItem("Tools/머티리얼/텍스처 자동 연결")]
        public static void ShowWindow()
        {
            AutoAssignTexture window = GetWindow<AutoAssignTexture>("텍스처 자동 연결");
            window.minSize = new Vector2(320f, 220f);
            window.maxSize = new Vector2(320f, 220f);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("쉐이더 프로퍼티 설정", EditorStyles.boldLabel);

            // 파이프라인 선택
            _propertyMode = (EPropertyName)EditorGUILayout.EnumPopup("렌더 파이프라인", _propertyMode);

            // 가이드
            string currentProperty = _propertyMode == EPropertyName.URP ? "_BaseMap" : "_MainTex";
            EditorGUILayout.HelpBox($"현재 '{currentProperty}' 프로퍼티에 텍스처가 연결됩니다.", MessageType.Info);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("사용 방법", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("프로젝트 창에서 머티리얼과 텍스처를 함께 선택(또는 두 폴더를 함께 선택)한 후 아래 버튼을 누르세요.\n\n확장자와 무관하게 '파일명'이 완전히 동일한 것끼리 짝지어집니다.", MessageType.None);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // 버튼 인식
            if (GUILayout.Button("동일한 이름의 텍스처 연결", GUILayout.Height(30)))
            {
                ExecuteAssign();
            }
        }
        #endregion

        #region ─────────────────────────▷ 실행 ◁─────────────────────────
        private void ExecuteAssign()
        {
            // 폴더 내부 포함
            UnityEngine.Material[] selectedMaterials = Selection.GetFiltered<UnityEngine.Material>(SelectionMode.DeepAssets);
            Texture[] selectedTextures = Selection.GetFiltered<Texture>(SelectionMode.DeepAssets);

            if (selectedMaterials.Length == 0 || selectedTextures.Length == 0)
            {
                Debug.LogWarning("[AutoAssign] 연결할 머티리얼과 텍스처(또는 해당 폴더들)를 함께 선택해주세요.");
                return;
            }

            // 텍스처 딕셔너리 작성
            Dictionary<string, Texture> textureDict = new Dictionary<string, Texture>();
            foreach (Texture tex in selectedTextures)
            {
                if (!textureDict.ContainsKey(tex.name))
                {
                    textureDict.Add(tex.name, tex);
                }
            }

            int successCount = 0;
            string targetProperty = _propertyMode == EPropertyName.URP ? "_BaseMap" : "_MainTex"; // 셰이더 프로퍼티 결정

            // 동일한 이름끼리 연결
            foreach (UnityEngine.Material mat in selectedMaterials)
            {
                if (textureDict.TryGetValue(mat.name, out Texture matchingTexture))
                {
                    // Ctrl Z 기록
                    Undo.RecordObject(mat, "Assign Texture to Material");
                    mat.SetTexture(targetProperty, matchingTexture);

                    // 변경 사항 저장 대기
                    EditorUtility.SetDirty(mat);
                    successCount++;
                }
            }

            if (successCount > 0)
            {
                AssetDatabase.SaveAssets();
                Debug.Log($"[AutoAssign] 총 {successCount}개의 머티리얼에 '{targetProperty}' 텍스처를 성공적으로 연결했습니다.");
            }
            else
            {
                Debug.LogWarning("[AutoAssign] 이름이 일치하는 머티리얼-텍스처 쌍을 찾지 못했습니다.");
            }
        }
        #endregion

        #region ─────────────────────────▷ 중첩 타입 ◁─────────────────────────
        private enum EPropertyName
        {
            BuiltIn = 0,
            URP = 1
        }
        #endregion
    }
}
