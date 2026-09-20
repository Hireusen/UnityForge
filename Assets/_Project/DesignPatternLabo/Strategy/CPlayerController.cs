using UnityEngine;

namespace Project.DesignPatternLabo
{
    public class CPlayerController : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("참조 연결")]
        [SerializeField] private CPlayerObject _player;
        [SerializeField] private CEnemyObject _enemy;

        [Header("명령 키")]
        [SerializeField] private KeyCode _physicalKey = KeyCode.Q;
        [SerializeField] private KeyCode _magicalKey = KeyCode.W;
        [SerializeField] private KeyCode _selfHarmKey = KeyCode.E;
        [SerializeField] private KeyCode _ultimateKey = KeyCode.R;
        [SerializeField] private KeyCode _attackKey = KeyCode.Space;

        private PlayerStrategyHub _playerHub;
        private IWeapon _physicalStrategy;
        private IWeapon _magicalStrategy;
        private IWeapon _selfHarmStrategy;
        private IWeapon _ultimateStrategy;
        #endregion

        private void Awake()
        {
            if (_enemy == null) Debug.Log("적이 연결되지 않았습니다.");

            _playerHub = new PlayerStrategyHub();
            _physicalStrategy = new PhysicalStrategy();
            _magicalStrategy = new MagicalStrategy();
            _selfHarmStrategy = new SelfHarmStrategy();
            _ultimateStrategy = new UltimateStrategy();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_physicalKey))
            {
                _playerHub.SetStrategy(_physicalStrategy);
            }
            else if (Input.GetKeyDown(_magicalKey))
            {
                _playerHub.SetStrategy(_magicalStrategy);
            }
            else if (Input.GetKeyDown(_selfHarmKey))
            {
                _playerHub.SetStrategy(_selfHarmStrategy);
            }
            else if (Input.GetKeyDown(_ultimateKey))
            {
                _playerHub.SetStrategy(_ultimateStrategy);
            }
            else if (Input.GetKeyDown(_attackKey))
            {
                _playerHub.Attack(_player, _enemy);
            }
        }
    }
}
