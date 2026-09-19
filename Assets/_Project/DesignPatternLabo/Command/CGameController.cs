using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Project.DesignPatternLabo
{
    // 상하좌우 이동 게임 + Undo / Redo 가능
    public class CGameController : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("이동 주체")]
        [SerializeField] private CMoveObject _player;

        [Header("키 설정")]
        [SerializeField] private KeyCode _upKey = KeyCode.W;
        [SerializeField] private KeyCode _downKey = KeyCode.S;
        [SerializeField] private KeyCode _leftKey = KeyCode.A;
        [SerializeField] private KeyCode _rightKey = KeyCode.D;
        [SerializeField] private KeyCode _undoKey = KeyCode.U;
        [SerializeField] private KeyCode _redoKey = KeyCode.R;
        [SerializeField] private KeyCode _replayKey = KeyCode.Tab;

        [Header("상태이상")]
        [SerializeField] private bool _reverseStatus = false;


        private ACommand _buttonUp;
        private ACommand _buttonDown;
        private ACommand _buttonLeft;
        private ACommand _buttonRight;

        private Stack<ACommand> _undoCommands = new();
        private Stack<ACommand> _redoCommands = new();

        private Vector3 _startPos;
        private bool _isReplaying = false;
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private void ExecuteNewCommand(ACommand command)
        {
            command.Execute();
            _undoCommands.Push(command);
            _redoCommands.Clear();
        }

        private void ExecuteRedo()
        {
            if (_redoCommands.Count <= 0) return;

            ACommand command = _redoCommands.Pop();
            command.Execute();
            _undoCommands.Push(command);
        }

        private void ExecuteUndo()
        {
            if (_undoCommands.Count <= 0) return;

            ACommand command = _undoCommands.Pop();
            command.Undo();
            _redoCommands.Push(command);
        }

        private void ExecuteReplay()
        {
            _isReplaying = true;
            _player.transform.position = _startPos;

            StartCoroutine(ReplayQueue());
        }

        private IEnumerator ReplayQueue()
        {
            ACommand[] oldCommands = _undoCommands.ToArray();

            for (int i = oldCommands.Length - 1; i >= 0; --i)
            {
                ACommand current = oldCommands[i];
                current.Execute();
                yield return new WaitForSeconds(0.3f);
            }

            _isReplaying = false;
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        private void Start()
        {
            _startPos = _player.transform.position;
            _buttonUp = new CMoveUpCommand(_player);
            _buttonDown = new CMoveDownCommand(_player);
            _buttonLeft = new CMoveLeftCommand(_player);
            _buttonRight = new CMoveRightCommand(_player);
        }
        private void Update()
        {
            if (_isReplaying) return;

            #region ─────────────────────────▷ 이동 처리 ◁─────────────────────────
            if (_reverseStatus)
            {
                if (Input.GetKeyDown(_upKey))
                {
                    ExecuteNewCommand(_buttonDown);
                }
                else if (Input.GetKeyDown(_downKey))
                {
                    ExecuteNewCommand(_buttonUp);
                }
                else if (Input.GetKeyDown(_leftKey))
                {
                    ExecuteNewCommand(_buttonRight);
                }
                else if (Input.GetKeyDown(_rightKey))
                {
                    ExecuteNewCommand(_buttonLeft);
                }
            }
            else
            {
                if (Input.GetKeyDown(_upKey))
                {
                    ExecuteNewCommand(_buttonUp);
                }
                else if (Input.GetKeyDown(_downKey))
                {
                    ExecuteNewCommand(_buttonDown);
                }
                else if (Input.GetKeyDown(_leftKey))
                {
                    ExecuteNewCommand(_buttonLeft);
                }
                else if (Input.GetKeyDown(_rightKey))
                {
                    ExecuteNewCommand(_buttonRight);
                }
            }
            #endregion

            if (Input.GetKeyDown(_undoKey))
            {
                ExecuteUndo();
            }
            if (Input.GetKeyDown(_redoKey))
            {
                ExecuteRedo();
            }
            if (Input.GetKeyDown(_replayKey))
            {
                ExecuteReplay();
            }
        }
        #endregion
    }
}
