using System;
using UnityEngine;

namespace Project.Default
{
    public class CActionCommand : IInputCommand
    {
        private readonly Action _action;

        public CActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action?.Invoke();
        }
    }
}
