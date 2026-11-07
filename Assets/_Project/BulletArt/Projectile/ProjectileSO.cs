using UnityEngine;

namespace Project.BulletArt
{
    [CreateAssetMenu(fileName = "Projectile_", menuName = "Scriptable Objects/BulletArt/Projectile")]
    public class ProjectileSO : ScriptableObject
    {
        [Header("탄막 기본 설정")]
        [SerializeField] private Sprite _image;
        [SerializeField] private int _damage;
        [SerializeField] private float _hitRadius;
        [SerializeField] private float _lifeTime;
        [SerializeField] private float _startDelay;

        [Header("탄막 이동 설정")] // 초당
        [SerializeField] private float _startSpeed = 5f;
        [SerializeField] private float _acceleration = 0f; // 음수일 경우 감속
        [SerializeField] private Vector2 _clampSpeed = new Vector2(0f, 20f);
        [SerializeField] private float _angularVelocity = 0f; // 회전 각도

        public Sprite Image => _image;
        public int Damage => _damage;
        public float HitRadius => _hitRadius;
        public float LifeTime => _lifeTime;
        public float StartDelay => _startDelay;
        public float StartSpeed => _startSpeed;
        public float Acceleration => _acceleration;
        public Vector2 ClampSpeed => _clampSpeed;
        public float AngularVelocity => _angularVelocity;
    }
}
