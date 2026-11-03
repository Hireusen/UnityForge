using UnityEngine;

namespace Project.BulletArt
{
    [CreateAssetMenu(fileName = "PilotBomb_", menuName = "Scriptable Objects/BulletArt/Bomb")]
    public class PilotBombSO : ScriptableObject
    {
        [Header("폭탄 설정")]
        [SerializeField] private EBombType _type;
        [SerializeField] private float _range;
        [SerializeField] private float _duration;

        public EBombType Type => _type;
        public float Range => _range;
        public float Duration => _duration;
    }

    public enum EBombType
    {
        Circle,
        Square,
    }
}
