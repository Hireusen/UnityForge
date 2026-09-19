using UnityEditor;
using UnityEngine;

namespace Plugins.ThirdParty
{
    public class ThirdPartyReferenceBlocker : AssetModificationProcessor
    {
        // 제한할 폴더 경로
        private const string THIRD_PARTY_PATH = "Assets/ThirdParty";
        private const string MENU_PATH = "Tools/서드파티/참조 방어";
        private static bool IsEnable // 유니티 꺼도 유지
        {
            get => EditorPrefs.GetBool("ThirdPartyReferenceBlocker.IsEnable", true);
            set => EditorPrefs.SetBool("ThirdPartyReferenceBlocker.IsEnable", value);
        }

        [MenuItem(MENU_PATH)]
        public static void ToggleReferenceBlocker()
        {
            IsEnable = !IsEnable;
            string status = IsEnable ? "활성화" : "비활성화";
            Debug.Log($"서드파티 참조 방어를 {status}되었습니다.");
        }

        [MenuItem(MENU_PATH, true)]
        public static bool ToggleReferenceBlockerValidate()
        {
            Menu.SetChecked(MENU_PATH, IsEnable);
            return true;
        }

        // 에셋이 저장되기 직전에 호출되는 유니티 내장 콜백 함수
        public static string[] OnWillSaveAssets(string[] paths)
        {
            if (!IsEnable) return paths;

            bool hasViolation = false;
            string violationMessage = "";

            foreach (string path in paths)
            {
                // 스크립트 저장, 서드파티 내부의 저장 무시
                if (path.EndsWith(".cs") || path.StartsWith(THIRD_PARTY_PATH)) continue;

                // 저장 대상이 의존하는 에셋 경로 추출
                string[] dependencies = AssetDatabase.GetDependencies(path, false);

                for (int i = 0; i < dependencies.Length; ++i)
                {
                    string dependency = dependencies[i];
                    // 자기 자신이 아니고, ThirdParty 폴더를 참조하고 있다면
                    if (dependency != path && dependency.StartsWith(THIRD_PARTY_PATH))
                    {
                        hasViolation = true;
                        violationMessage += $"\nㄴ'{path}'가 '{dependency}'를 참조하고 있습니다.";
                    }
                }
            }

            // 위반 발생
            if (hasViolation)
            {
                string errorMessage = "🚨 ThirdParty 에셋 참조 에러 🚨" +
                                      "\nThirdParty 폴더의 에셋은 직접 사용할 수 없습니다." +
                                      "\n반드시 프로젝트 폴더로 복사한 후 사용해주세요." +
                                      violationMessage;
                Debug.LogWarning(errorMessage);
                EditorUtility.DisplayDialog("저장 실패 (외부 에셋 참조됨)", errorMessage, "확인");
            }

            return paths;
        }
    }
}
