using System.ComponentModel;
using UnityEngine;

namespace Project.DesignPatternLabo
{
    public class CClient : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("키 설정")]
        [SerializeField] private KeyCode _useStaticKey = KeyCode.Q;
        [SerializeField] private KeyCode _useSingletonKey = KeyCode.W;
        [SerializeField] private KeyCode _addScoreKey = KeyCode.E;
        [SerializeField] private KeyCode _getScoreKey = KeyCode.R;

        [Header("점수 증가량")]
        [SerializeField] private int _scoreChangeAmount = 100;

        private EManagerType _managerType;
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Update()
        {
            if (Input.GetKeyDown(_useStaticKey))
            {
                _managerType = EManagerType.Static;
                Debug.Log($"사용할 매니저 타입을 {_managerType}로 설정했습니다.");
            }
            if (Input.GetKeyDown(_useSingletonKey))
            {
                _managerType = EManagerType.Singleton;
                Debug.Log($"사용할 매니저 타입을 {_managerType}로 설정했습니다.");
            }
            if (Input.GetKeyDown(_addScoreKey))
            {
                if (_managerType == EManagerType.Singleton)
                {
                    CSingletonManager.Instance.AddScore(_scoreChangeAmount);
                }
                else if (_managerType == EManagerType.Static)
                {
                    CStaticManager.AddScore(_scoreChangeAmount);
                }
            }
            if (Input.GetKeyDown(_getScoreKey))
            {
                int currentScore = 0;
                if (_managerType == EManagerType.Singleton)
                {
                    currentScore = CSingletonManager.Instance.GetScore();
                }
                else if (_managerType == EManagerType.Static)
                {
                    currentScore = CStaticManager.GetScore();
                }
                Debug.Log($"현재 {_managerType}의 점수는 {currentScore}입니다.");
            }
        }
        #endregion

        #region ─────────────────────────▷ 중첩 타입 ◁─────────────────────────
        private enum EManagerType
        {
            Singleton,
            Static
        }
        #endregion
    }
}
