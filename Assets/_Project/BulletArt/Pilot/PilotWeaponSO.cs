using UnityEngine;
using System.Collections.Generic;

namespace Project.BulletArt
{
    [CreateAssetMenu(fileName = "PilotWeapon_", menuName = "Scriptable Objects/BulletArt/Weapon")]
    public class PilotWeaponSO : ScriptableObject
    {
        [Header("무기 설정")]
        [SerializeField] private float _delay;
        [SerializeField] private ProjectileInfo[] _bulletInfos;

        public float Delay => _delay;
        public IReadOnlyList<ProjectileInfo> BulletInfos => _bulletInfos;

        [System.Serializable]
        public struct ProjectileInfo
        {
            public ProjectileSO so;
            public Vector2 spawnPos;
            public float spawnAngle;
        }
    }
}
