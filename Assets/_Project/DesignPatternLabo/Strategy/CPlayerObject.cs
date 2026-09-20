using System.Collections;
using UnityEngine;

namespace Project.DesignPatternLabo
{
    [RequireComponent(typeof(Renderer))]
    public class CPlayerObject : MonoBehaviour, IEntity
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("능력치")]
        [SerializeField] private int _health = 1000;
        [SerializeField] private int _physical = 100;
        [SerializeField] private int _magical = 200;

        private Vector3 _originScale;
        #endregion

        public int Health => _health;
        public int Physical => _physical;
        public int Magical => _magical;

        private void Awake()
        {
            _originScale = transform.localScale;
        }

        public void Hit(int damage)
        {
            _health -= damage;
            Debug.Log($"플레이어는 {damage}의 대미지를 받았다! (남은 체력 : {_health})");
            TryDie();
            StartCoroutine(HitEffect());
        }

        private IEnumerator HitEffect()
        {
            const float DURATION = 1f;
            const float MOVEMENT = 0.01f;
            const float SCALE_MUL = 1.01f;

            float targetTime = Time.time + DURATION;
            while(Time.time < targetTime)
            {
                transform.localScale *= SCALE_MUL;
                transform.Translate(MOVEMENT * Vector3.back);
                yield return null;
            }

            transform.localScale = _originScale;
        }

        private void TryDie()
        {
            if (_health > 0) return;

            Debug.Log("플레이어는 쓰러졌다...");
            Destroy(gameObject);
        }
    }
}
