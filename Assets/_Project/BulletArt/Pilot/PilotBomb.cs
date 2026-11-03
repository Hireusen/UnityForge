using UnityEngine;

namespace Project.BulletArt
{
    [System.Serializable]
    public class PilotBomb
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("파일럿 이동")]
        [SerializeField] private Transform _pilotTr;
        [SerializeField] private PilotBombSO _bomb;

        #endregion

        #region ─────────────────────────▷ 공개 멤버 ◁─────────────────────────
        public void Execute()
        {
            if(_pilotTr == null || _bomb == null) return;

        }
        #endregion
    }
}
