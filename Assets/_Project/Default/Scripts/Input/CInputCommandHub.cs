using UnityEngine;

namespace Project.Default
{
    public class CInputCommandHub : MonoBehaviour
    {
        // 뉴 인풋은 커질수록 입력 + 동작이 여러 핸들러에 퍼지는 구조

        private void HandlePause()
        {
            // Execute("Pause.Toggle");
        }

        private void BindInputEvents(CInputDispatcher idspatcher)
        {

        }

        private class CPauseToogleCommand : IInputCommand
        {
            public void Execute()
            {
                
            }
        }
    }
}
