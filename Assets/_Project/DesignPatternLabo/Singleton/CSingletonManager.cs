using UnityEngine;

namespace Project.DesignPatternLabo
{
    public class CSingletonManager : MonoBehaviour
    {
        public static CSingletonManager Instance { get; private set; }

        #region ─────────────────────────▷ 점수 ◁─────────────────────────
        private static int _score = 0;

        public void AddScore(int amount)
        {
            _score += amount;
            Debug.Log($"싱글톤 매니저를 통해 점수를 {amount}점 추가했습니다.");
        }

        public int GetScore() => _score;
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Awake()
        {
            if (Instance == null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                Destroy(gameObject);
            }
        }
        #endregion
    }
}
