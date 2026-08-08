using Project.Default;
using UnityEngine;

namespace Project.Performance
{
    /// <summary>
    /// 배열의 순회 방식과 구조에 따른 성능 차이를 테스트하는 컴포넌트입니다.
    /// </summary>
    public class CTestArray : AMono
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("테스트 값 (실제 크기는 size * size)")]
        [SerializeField] private int _size = 10000;

        private static int _dummy = 0;
        // 결과 출력을 위한 상수 이름 정의
        private const string NAME_2D = "2차원 배열 [,]";
        private const string NAME_1D = "1차원 평탄화 배열 []";
        #endregion

        #region ─────────────────────────▷ 컨텍스트 함수 ◁─────────────────────────
        [ContextMenu("배열 Y-X 순회와 X-Y 순회의 속도를 비교합니다.")]
        private void TestTraversalOrder()
        {
            // 준비
            const string NAME_YX = "YX 순회";
            const string NAME_XY = "XY 순회";
            int size = _size;
            int[,] array2D = Create2DArray();
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_YX, result, 0)) // Y-X
            {
                int localDummy = 0;
                for (int y = 0; y < size; ++y)
                {
                    for (int x = 0; x < size; ++x)
                    {
                        localDummy += array2D[y, x];
                    }
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_XY, result, 1)) // X-Y
            {
                int localDummy = 0;
                for (int x = 0; x < size; ++x)
                {
                    for (int y = 0; y < size; ++y)
                    {
                        localDummy += array2D[y, x];
                    }
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_YX, result[0], NAME_XY, result[1]);
        }

        [ContextMenu("2차원 배열과 1차원 평탄화 배열의 순회 속도를 비교합니다.")]
        private void TestDimensionType()
        {
            // 준비
            int size = _size;
            int[,] array2D = Create2DArray();
            int[] array1D = Create1DArray();
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_2D, result, 0)) // 2차원 배열
            {
                int localDummy = 0;
                for (int y = 0; y < size; ++y)
                {
                    for (int x = 0; x < size; ++x)
                    {
                        localDummy += array2D[y, x];
                    }
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_1D, result, 1)) // 1차원 평탄화 배열
            {
                int localDummy = 0;
                for (int y = 0; y < size; ++y)
                {
                    // 행 시작 인덱스
                    int rowIndex = y * size;

                    for (int x = 0; x < size; ++x)
                    {
                        localDummy += array1D[rowIndex + x];
                    }
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_2D, result[0], NAME_1D, result[1]);
        }

        [ContextMenu("2차원 배열과 가변 배열의 속도를 비교합니다.")]
        private void TestJaggedVs2D()
        {
            // 준비
            const string NAME_JAGGED = "가변 배열 [][]";
            int size = _size;
            int[,] array2D = Create2DArray();
            int[][] arrayJagged = CreateJaggedArray();
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_2D, result, 0)) // 2차원 배열
            {
                int localDummy = 0;
                for (int y = 0; y < size; ++y)
                {
                    for (int x = 0; x < size; ++x)
                    {
                        localDummy += array2D[y, x];
                    }
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_JAGGED, result, 1)) // 가변 배열
            {
                int localDummy = 0;
                for (int y = 0; y < size; ++y)
                {
                    for (int x = 0; x < size; ++x)
                    {
                        localDummy += arrayJagged[y][x];
                    }
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_JAGGED, result[1], NAME_2D, result[0]);
        }

        [ContextMenu("1차원 배열의 For문과 Foreach문 속도를 비교합니다.")]
        private void TestForVsForeach()
        {
            // 준비
            const string NAME_FOR = "For 루프";
            const string NAME_FOREACH = "Foreach 루프";
            int size = _size;
            int[] array1D = Create1DArray();
            int totalLength = size * size;
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_FOR, result, 0)) // for문
            {
                int localDummy = 0;
                for (int i = 0; i < totalLength; ++i)
                {
                    localDummy += array1D[i];
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_FOREACH, result, 1)) // foreach문
            {
                int localDummy = 0;
                foreach (int value in array1D)
                {
                    localDummy += value;
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_FOR, result[0], NAME_FOREACH, result[1]);
        }
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private int[,] Create2DArray()
        {
            int size = _size;
            int[,] arr = new int[size, size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    arr[y, x] = 1;
                }
            }

            return arr;
        }

        private int[] Create1DArray()
        {
            int size = _size;
            int totalSize = size * size;
            int[] arr = new int[totalSize];
            for (int i = 0; i < totalSize; i++)
            {
                arr[i] = 1;
            }

            return arr;
        }

        private int[][] CreateJaggedArray()
        {
            int size = _size;
            int[][] arr = new int[size][];
            for (int y = 0; y < size; y++)
            {
                arr[y] = new int[size];
                for (int x = 0; x < size; x++)
                {
                    arr[y][x] = 1;
                }
            }
            return arr;
        }
        #endregion
    }
}
