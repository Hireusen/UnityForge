using Project.Default;
using UnityEngine;

namespace Project.Performance
{
    /// <summary>
    /// 기본 연산자 성능 차이 테스트
    /// </summary>
    public class CTestOperator : AMono
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("테스트 반복 횟수")]
        [SerializeField] private int _loopCount = 10000000;

        private static int _dummyInt = 0;
        private static float _dummyFloat = 0f;
        #endregion

        #region ─────────────────────────▷ 컨텍스트 함수 ◁─────────────────────────
        [ContextMenu("곱셈과 나눗셈의 연산 속도를 비교합니다.")]
        public void TestMultiplyAndDivide()
        {
            // 준비
            const string NAME_MULT = "곱셈 (* 0.5f)";
            const string NAME_DIV = "나눗셈 (/ 2.0f)";
            int loopCount = _loopCount;
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_MULT, result, 0)) // 곱셈
            {
                float localDummy = 0f;
                for (int i = 1; i <= loopCount; ++i)
                {
                    localDummy += i * 0.5f;
                }
                _dummyFloat += localDummy;
            }
            using (new UTimer(NAME_DIV, result, 1)) // 나눗셈
            {
                float localDummy = 0f;
                for (int i = 1; i <= loopCount; ++i)
                {
                    localDummy += i / 2.0f;
                }
                _dummyFloat += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummyFloat, NAME_MULT, result[0], NAME_DIV, result[1]);
        }

        [ContextMenu("1을 더하는 것과 1000을 더하는 것의 속도를 비교합니다.")]
        public void TestAddSmallAndLargeNumber()
        {
            // 준비
            const string NAME_ADD_1 = "1 더하기 (+ 1)";
            const string NAME_ADD_1000 = "1000 더하기 (+ 1000)";
            int loopCount = _loopCount;
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_ADD_1, result, 0)) // 1 더하기
            {
                int localDummy = 0;
                for (int i = 0; i < loopCount; ++i)
                {
                    localDummy += 1;
                }
                _dummyInt += localDummy;
            }
            using (new UTimer(NAME_ADD_1000, result, 1)) // 1000 더하기
            {
                int localDummy = 0;
                for (int i = 0; i < loopCount; ++i)
                {
                    localDummy += 1000;
                }
                _dummyInt += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummyInt, NAME_ADD_1, result[0], NAME_ADD_1000, result[1]);
        }

        [ContextMenu("비트 시프트 연산과 곱셈 연산의 속도를 비교합니다.")]
        public void TestBitwiseShiftAndMultiply()
        {
            // 준비
            const string NAME_BITWISE = "비트 시프트 (<< 1)";
            const string NAME_MULT = "곱셈 (* 2)";
            int loopCount = _loopCount;
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_BITWISE, result, 0)) // 비트 시프트
            {
                int localDummy = 0;
                for (int i = 0; i < loopCount; ++i)
                {
                    localDummy += i << 1;
                }
                _dummyInt += localDummy;
            }
            using (new UTimer(NAME_MULT, result, 1)) // 정수 곱셈
            {
                int localDummy = 0;
                for (int i = 0; i < loopCount; ++i)
                {
                    localDummy += i * 2;
                }
                _dummyInt += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummyInt, NAME_BITWISE, result[0], NAME_MULT, result[1]);
        }

        [ContextMenu("나머지 연산(%)과 비트 논리곱(&) 연산의 속도를 비교합니다.")]
        public void TestModuloAndBitwiseAnd()
        {
            // 준비
            const string NAME_MODULO = "나머지 연산 (% 8)";
            const string NAME_BITWISE = "비트 논리곱 (& 7)";
            int loopCount = _loopCount;
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_MODULO, result, 0)) // 나머지 연산
            {
                int localDummy = 0;
                for (int i = 0; i < loopCount; ++i)
                {
                    localDummy += i % 8;
                }
                _dummyInt += localDummy;
            }
            using (new UTimer(NAME_BITWISE, result, 1)) // 비트 논리곱 연산
            {
                int localDummy = 0;
                for (int i = 0; i < loopCount; ++i)
                {
                    localDummy += i & 7;
                }
                _dummyInt += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummyInt, NAME_BITWISE, result[1], NAME_MODULO, result[0]);
        }

        [ContextMenu("Mathf.Pow 함수와 직접 곱하기의 속도를 비교합니다.")]
        public void TestPowAndDirectMultiply()
        {
            // 준비
            const string NAME_POW = "Mathf.Pow(x, 2)";
            const string NAME_DIRECT = "직접 곱하기 (x * x)";
            int loopCount = _loopCount;
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_POW, result, 0)) // 내장 함수 사용
            {
                float localDummy = 0f;
                for (int i = 0; i < loopCount; ++i)
                {
                    localDummy += Mathf.Pow(i, 2);
                }
                _dummyFloat += localDummy;
            }
            using (new UTimer(NAME_DIRECT, result, 1)) // 직접 곱셈
            {
                float localDummy = 0f;
                for (int i = 0; i < loopCount; ++i)
                {
                    localDummy += i * i;
                }
                _dummyFloat += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummyFloat, NAME_DIRECT, result[1], NAME_POW, result[0]);
        }
        #endregion
    }
}
