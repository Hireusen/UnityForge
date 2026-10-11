using System.Collections.Generic;
using UnityEngine;

namespace Project.Default
{
    // 컬렉션이 붙으면 커맨드를 중앙 등록을 해놓고 꺼내서 쓸 수 있다.
    public class CGenericCommandHub : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        // Set으로 한 번에 처리 가능
        private readonly Dictionary<string, IInputCommand> _commands = new();
        private readonly Dictionary<string, IInputCommand<Vector2>> _vectorCommands = new();
        private readonly Dictionary<string, IInputCommand<float>> _floatCommands = new();

        // 공통 부모 선언해서 한번에 처리 가능
        // 대신 꺼낼 때 타입 확인하고 꺼내야함
        private readonly Dictionary<string, ICommand> _aCommands = new();
        #endregion

        /*
        public void Execute(string key, Vector2 value)
        {
            if(_aCommands.TryGetValue(key, out ICommand command) == false){
                return;
            }

            if(command is IInputCommand<Vector2> valueCommand)
            {
                valueCommand.Execute(value);
            }
        }
        */

        public void Register(string key, IInputCommand command)
        {
            _commands[key] = command;
        }

        public void Register(string key, IInputCommand<Vector2> command)
        {
            _vectorCommands[key] = command;
        }

        public void Register(string key, IInputCommand<float> command)
        {
            _floatCommands[key] = command;
        }

        public void Execute(string key)
        {
            if (_commands.TryGetValue(key, out IInputCommand command))
            {
                command.Execute();
            }
        }

        public void Execute(string key, Vector2 value)
        {
            if(_vectorCommands.TryGetValue(key, out IInputCommand<Vector2> command))
            {
                command.Execute(value);
            }
        }

        public void Execute(string key, float value)
        {
            if (_floatCommands.TryGetValue(key, out IInputCommand<float> command))
            {
                command.Execute(value);
            }
        }
    }
}
