using Project.Default;
using UnityEngine;
using System.Buffers;

namespace Project.Performance
{
    /// <summary>
    /// 배열 성능 테스트
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
        [ContextMenu("배열 생성과 배열 대여 속도를 비교합니다.")]
        public void TestNewArrayAndRent()
        {
            // 준비
            const string NAME_NEW = "배열 생성";
            const string NAME_POOL = "배열 대여";
            const int LOOP_COUNT = 10000;
            int size = _size;
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_NEW, result, 0)) // 새로 할당
            {
                for(int i = 0; i < LOOP_COUNT; ++i)
                {
                    int[] array1D = UCollectionBuilder.Create1DArray(size);
                    
                }
            }
            using (new UTimer(NAME_POOL, result, 1)) // 대여
            {
                for (int i = 0; i < LOOP_COUNT; ++i)
                {
                    int[] array1D = ArrayPool<int>.Shared.Rent(size);
                    
                    ArrayPool<int>.Shared.Return(array1D);
                }
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_NEW, result[0], NAME_POOL, result[1]);
        }

        [ContextMenu("배열 Y-X 순회와 X-Y 순회의 속도를 비교합니다.")]
        public void TestTraversalOrder()
        {
            // 준비
            const string NAME_YX = "YX 순회";
            const string NAME_XY = "XY 순회";
            int size = _size;
            int[,] array2D = UCollectionBuilder.Create2DArray(size);
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

        [ContextMenu("일차원 배열과 가변 배열의 순회 속도를 비교합니다.")]
        public void Test1DAndJagged()
        {
            // 준비
            const string NAME_1D = "일차원 배열";
            const string NAME_JAGGED = "가변 배열 순회";
            int size = _size;
            int totalSize = size * size;
            int[] array1D = UCollectionBuilder.Create1DArray(totalSize);
            int[][] arrayJagged = UCollectionBuilder.CreateJaggedArray(size);
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_1D, result, 0)) // 일차원 배열
            {
                int localDummy = 0;
                for(int i = 0; i < totalSize; ++i)
                {
                    localDummy += array1D[i];
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
            UTimer.CompareWithDummy(_dummy, NAME_1D, result[0], NAME_JAGGED, result[1]);
        }

        [ContextMenu("2차원 배열과 1차원 평탄화 배열의 순회 속도를 비교합니다.")]
        public void TestDimensionType()
        {
            // 준비
            int size = _size;
            int totalSize = size * size;
            int[,] array2D = UCollectionBuilder.Create2DArray(size);
            int[] array1D = UCollectionBuilder.Create1DArray(totalSize);
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
        public void TestJaggedVs2D()
        {
            // 준비
            const string NAME_JAGGED = "가변 배열 [][]";
            int size = _size;
            int[,] array2D = UCollectionBuilder.Create2DArray(size);
            int[][] arrayJagged = UCollectionBuilder.CreateJaggedArray(size);
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
        #endregion
    }
}
