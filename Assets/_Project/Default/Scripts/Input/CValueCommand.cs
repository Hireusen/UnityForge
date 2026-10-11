using System;

namespace Project.Default
{
    public class CValueCommand<T> : IInputCommand<T>
    {
        private readonly Action<T> _action;

        public CValueCommand(Action<T> action) {
            _action = action;
        }

        public void Execute(T value)
        {
            _action?.Invoke(value);
        }
    }
}
