using UnityEngine;

namespace Project.DesignPatternLabo
{
    public static class CStaticManager
    {
        private static int _score = 0;

        public static void AddScore(int amount)
        {
            _score += amount;
            Debug.Log($"정적 매니저를 통해 점수를 {amount}점 추가했습니다.");
        }

        public static int GetScore() => _score;
    }
}
