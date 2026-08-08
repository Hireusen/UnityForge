using UnityEditor;

namespace Project.Default
{
    public class CHierarchySeparator : AMono
    {
#if UNITY_EDITOR
        private const string SEPARATOR_NAME = "──────────────";
        private void OnValidate()
        {
            EditorApplication.delayCall += SetGameObjectName;
        }
        private void SetGameObjectName()
        {
            if (this == null || gameObject == null) return;

            if (gameObject.name != SEPARATOR_NAME)
            {
                gameObject.name = SEPARATOR_NAME;
            }
        }
    }
#endif
}
