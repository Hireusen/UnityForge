using UnityEngine;

namespace Project.BulletArt
{
    [CreateAssetMenu(fileName = "PilotMove_", menuName = "Scriptable Objects/BulletArt/Move")]
    public class PilotMoveSO : ScriptableObject
    {
        [Header("이동 설정")]
        [SerializeField] private float _highSpeed;
        [SerializeField] private float _lowSpeed;

        public float HighSpeed => _highSpeed;
        public float LowSpeed => _lowSpeed;
    }
}
