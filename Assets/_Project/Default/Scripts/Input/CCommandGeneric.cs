using UnityEngine;

namespace Project.Default
{
    public class CCommandGeneric : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("참조 연결")]
        [SerializeField] private CCommandPlayer _player;

        private IInputCommand _jumpCommand;
        private IInputCommand<Vector2> _moveCommand;
        private IInputCommand<float> _zoomCommand;
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Awake()
        {
            _jumpCommand = new CActionCommand(_player.Jump);
            _moveCommand = new CValueCommand<Vector2>(_player.SetMove);
            /*
            _moveCommand = new CValueCommand<Vector2>(
                value => _player.SetMove(value)
            );
            _moveCommand = new CValueCommand<Vector2>(
                value => {
                    Vector2 input = Vector2.ClampMagnitude(value, 1f);
                    _player.SetMove(input)
                }
            );
            */
            _zoomCommand = new CValueCommand<float>(_player.SetZoom);
        }
        private void Start()
        {
            
        }
        private void Update()
        {
            _moveCommand.Execute(new Vector2(0f, 1f));
            _zoomCommand.Execute(2f);
            _jumpCommand.Execute();
        }
        #endregion
    }
}
