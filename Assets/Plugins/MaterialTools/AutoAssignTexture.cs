using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Plugins.MaterialTools
{
    /// <summary>
    /// 선택한 에셋 중 이름이 일치하는 머티리얼과 텍스처를 연결시킵니다.
    /// </summary>
    public class AutoAssignTexture : EditorWindow
    {
        #region ─────────────────────────▷ 설정 값 ◁─────────────────────────
        // 렌더 파이프라인 선택
        private EPipelineMode _pipelineMode = EPipelineMode.URP;

        // 스크롤 위치 기록
        private Vector2 _scrollPos;

        // 텍스처 매핑 규칙 리스트
        private List<TextureMapRule> _rules = new List<TextureMapRule>()
        {
            new TextureMapRule(true, "메인(Albedo)", "", "_BaseMap", "_MainTex"),
            new TextureMapRule(true, "노멀(Normal)", "_Normal", "_BumpMap", "_BumpMap"),
            new TextureMapRule(true, "하이트(Height)", "_Height", "_ParallaxMap", "_ParallaxMap"),
            new TextureMapRule(true, "메탈릭(Metallic)", "_Metallic", "_MetallicGlossMap", "_MetallicGlossMap"),
            new TextureMapRule(true, "오클루전(AO)", "_AO", "_OcclusionMap", "_OcclusionMap"),
            new TextureMapRule(true, "에미션(Emission)", "_Emission", "_EmissionMap", "_EmissionMap")
        };
        #endregion

        #region ─────────────────────────▷ 윈도우 ◁─────────────────────────
        [MenuItem("Tools/머티리얼/텍스처 자동 연결")]
        public static void ShowWindow()
        {
            AutoAssignTexture window = GetWindow<AutoAssignTexture>("다중 텍스처 연결");
            window.minSize = new Vector2(420f, 400f);
            window.maxSize = new Vector2(420f, 400f);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("파이프라인 설정", EditorStyles.boldLabel);
            _pipelineMode = (EPipelineMode)EditorGUILayout.EnumPopup("렌더 파이프라인", _pipelineMode);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("매핑 규칙 (접미사 및 프로퍼티)", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("머티리얼 이름 뒤에 붙는 '접미사'를 바탕으로 텍스처를 찾습니다.\n예: 머티리얼이 'Rock'이면 '_Normal' 접미사 규칙은 'Rock_Normal' 텍스처를 찾습니다.", MessageType.Info);

            // 테이블 헤더
            GUILayout.BeginHorizontal();
            GUILayout.Label("사용", GUILayout.Width(30));
            GUILayout.Label("맵 종류", GUILayout.Width(80));
            GUILayout.Label("접미사(Suffix)", GUILayout.Width(80));
            GUILayout.Label("URP 속성", GUILayout.Width(100));
            GUILayout.Label("Built-in 속성", GUILayout.Width(100));
            GUILayout.EndHorizontal();

            // 리스트 스크롤 뷰
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(160));
            foreach (var rule in _rules)
            {
                GUILayout.BeginHorizontal();
                rule.isActive = EditorGUILayout.Toggle(rule.isActive, GUILayout.Width(30));
                GUILayout.Label(rule.displayName, GUILayout.Width(80));
                rule.suffix = EditorGUILayout.TextField(rule.suffix, GUILayout.Width(80));
                rule.urpProperty = EditorGUILayout.TextField(rule.urpProperty, GUILayout.Width(100));
                rule.builtInProperty = EditorGUILayout.TextField(rule.builtInProperty, GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();

            if (GUILayout.Button("선택 항목 텍스처 일괄 연결", GUILayout.Height(30)))
            {
                ExecuteAssign();
            }
        }
        #endregion

        #region ─────────────────────────▷ 실행 ◁─────────────────────────
        private void ExecuteAssign()
        {
            UnityEngine.Material[] selectedMaterials = Selection.GetFiltered<UnityEngine.Material>(SelectionMode.DeepAssets);
            Texture[] selectedTextures = Selection.GetFiltered<Texture>(SelectionMode.DeepAssets);

            if (selectedMaterials.Length == 0 || selectedTextures.Length == 0)
            {
                Debug.LogWarning("[AutoAssign] 연결할 머티리얼과 텍스처(또는 해당 폴더들)를 함께 선택해주세요.");
                return;
            }

            // 텍스처를 이름 기준으로 딕셔너리에 담기
            Dictionary<string, Texture> textureDict = new Dictionary<string, Texture>();
            foreach (Texture tex in selectedTextures)
            {
                if (!textureDict.ContainsKey(tex.name))
                {
                    textureDict.Add(tex.name, tex);
                }
            }

            int totalAssigned = 0;
            StringBuilder logBuilder = new StringBuilder();
            logBuilder.AppendLine("<b>[텍스처 자동 연결 결과]</b>");

            foreach (Material mat in selectedMaterials)
            {
                bool isMatModified = false;
                List<string> successLogs = new List<string>();
                List<string> errorLogs = new List<string>();

                foreach (var rule in _rules)
                {
                    if (!rule.isActive) continue;

                    string expectedTexName = mat.name + rule.suffix;
                    string targetProperty = _pipelineMode == EPipelineMode.URP ? rule.urpProperty : rule.builtInProperty;

                    if (textureDict.TryGetValue(expectedTexName, out Texture matchingTexture))
                    {
                        // 셰이더가 해당 프로퍼티를 지원하는지 확인
                        if (mat.HasProperty(targetProperty))
                        {
                            Undo.RecordObject(mat, "Assign Texture to Material");
                            mat.SetTexture(targetProperty, matchingTexture);

                            successLogs.Add($"<color=#4CAF50>✔ {rule.displayName}: {targetProperty} ({matchingTexture.name})</color>");
                            isMatModified = true;
                            totalAssigned++;
                        }
                        else
                        {
                            errorLogs.Add($"<color=#F44336>✖ {rule.displayName}: 셰이더가 '{targetProperty}' 프로퍼티를 지원하지 않음.</color>");
                        }
                    }
                }

                if (successLogs.Count > 0 || errorLogs.Count > 0)
                {
                    logBuilder.AppendLine($"\n<b>▶ 머티리얼: {mat.name}</b>");
                    foreach (var log in successLogs) logBuilder.AppendLine(log);
                    foreach (var log in errorLogs) logBuilder.AppendLine(log);
                }

                if (isMatModified)
                {
                    EditorUtility.SetDirty(mat);
                }
            }

            if (totalAssigned > 0)
            {
                AssetDatabase.SaveAssets();
                logBuilder.AppendLine($"\n<b>[완료] 총 {totalAssigned}개의 텍스처 슬롯이 성공적으로 연결되었습니다.</b>");
                Debug.Log(logBuilder.ToString());
            }
            else
            {
                Debug.LogWarning("[AutoAssign] 조건에 맞아 연결된 텍스처가 없습니다. 접미사 또는 셰이더 프로퍼티를 확인해주세요.");
            }
        }
        #endregion

        #region ─────────────────────────▷ 중첩 타입 ◁─────────────────────────
        private enum EPipelineMode
        {
            BuiltIn = 0,
            URP = 1
        }

        [Serializable]
        private class TextureMapRule
        {
            public bool isActive;
            public string displayName;
            public string suffix;
            public string urpProperty;
            public string builtInProperty;

            public TextureMapRule(bool isActive, string displayName, string suffix, string urpProperty, string builtInProperty)
            {
                this.isActive = isActive;
                this.displayName = displayName;
                this.suffix = suffix;
                this.urpProperty = urpProperty;
                this.builtInProperty = builtInProperty;
            }
        }
        #endregion
    }
}
