using UnityEngine;

namespace Project.BulletArt
{
    [System.Serializable]
    public class PilotAttack
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("공격 연결")]
        [SerializeField] private Transform _pilotTr;
        [SerializeField] private PilotWeaponSO _weapon;

        private float _nextAttackTime;
        #endregion

        #region ─────────────────────────▷ 공개 멤버 ◁─────────────────────────
        public void Execute()
        {
            if(_pilotTr == null || _weapon == null) return;
            if (Time.time < _nextAttackTime) return;

            _nextAttackTime = Time.time + _weapon.Delay;
        }
        #endregion
    }
}
