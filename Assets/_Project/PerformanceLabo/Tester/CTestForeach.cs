using System.Collections.Generic;
using UnityEngine;
using Project.Default;

namespace Project.Performance
{
    /// <summary>
    /// 다양한 컬렉션에서의 For문과 Foreach문, 
    /// 그리고 접근 방식에 따른 탐색 성능 차이를 테스트하는 컴포넌트입니다.
    /// </summary>
    public class CTestForeach : AMono
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("테스트 값 (실제 크기는 size * size)")]
        [SerializeField] private int _size = 10000;

        private static int _dummy = 0;

        // 결과 출력을 위한 상수 이름 정의
        private const string NAME_FOR = "For 루프";
        private const string NAME_FOREACH = "Foreach 루프";
        #endregion

        #region ─────────────────────────▷ 컨텍스트 함수 (기본 루프 비교) ◁─────────────────────────
        [ContextMenu("1차원 배열의 For문과 Foreach문 속도를 비교합니다.")]
        public void TestArrayForVsForeach()
        {
            // 준비
            int totalLength = _size * _size;
            int[] array1D = UCollectionBuilder.Create1DArray(totalLength);
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_FOR, result, 0))
            {
                int localDummy = 0;
                for (int i = 0; i < totalLength; ++i)
                {
                    localDummy += array1D[i];
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_FOREACH, result, 1))
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

        [ContextMenu("일반 List의 For문과 Foreach문 속도를 비교합니다.")]
        public void TestListForVsForeach()
        {
            // 준비
            int totalLength = _size * _size;
            List<int> list = UCollectionBuilder.CreateList(totalLength);
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_FOR, result, 0)) // for
            {
                int localDummy = 0;
                for (int i = 0; i < totalLength; ++i)
                {
                    localDummy += list[i];
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_FOREACH, result, 1)) // foreach
            {
                int localDummy = 0;
                foreach (int value in list)
                {
                    localDummy += value;
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_FOR, result[0], NAME_FOREACH, result[1]);
        }

        [ContextMenu("딕셔너리의 For문과 Foreach문 속도를 비교합니다.")]
        public void TestDictionaryForVsForeach()
        {
            // 준비
            int totalLength = _size * _size;
            Dictionary<int, int> dict = UCollectionBuilder.CreateDictionary(totalLength, _dummy);
            double[] result = new double[2];


            // 테스트 시작
            using (new UTimer(NAME_FOR, result, 0)) // 해시
            {
                int localDummy = 0;
                for (int i = 0; i < totalLength; ++i)
                {
                    localDummy += dict[i];
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_FOREACH, result, 1)) // 내부 배열
            {
                int localDummy = 0;
                foreach (var value in dict.Values)
                {
                    localDummy += value;
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_FOR, result[0], NAME_FOREACH, result[1]);
        }
        #endregion

        #region ─────────────────────────▷ 컨텍스트 함수 (심화 탐색 비교) ◁─────────────────────────
        [ContextMenu("딕셔너리의 Key 순회와 Value 순회 속도를 비교합니다.")]
        public void TestDictionaryKeyVsValue()
        {
            // 준비
            const string NAME_KEY = "Keys 순회";
            const string NAME_VALUE = "Values 순회";

            int totalLength = _size * _size;
            Dictionary<int, int> dict = UCollectionBuilder.CreateDictionary(totalLength, _dummy);
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_KEY, result, 0)) // Key
            {
                int localDummy = 0;
                foreach (int key in dict.Keys)
                {
                    localDummy += key;
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_VALUE, result, 1)) // Value
            {
                int localDummy = 0;
                foreach (int value in dict.Values)
                {
                    localDummy += value;
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_KEY, result[0], NAME_VALUE, result[1]);
        }

        [ContextMenu("딕셔너리의 Pair 순회와 단일 순회 속도를 비교합니다.")]
        public void TestDictionaryPairVsValue()
        {
            // 준비
            const string NAME_PAIR = "Pair 전체 순회";
            const string NAME_VALUE = "Values 단일 순회";

            int totalLength = _size * _size;
            Dictionary<int, int> dict = UCollectionBuilder.CreateDictionary(totalLength, _dummy);
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_PAIR, result, 0))
            {
                int localDummy = 0;
                foreach (var pair in dict) // 구조체 복사
                {
                    localDummy += pair.Value;
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_VALUE, result, 1))
            {
                int localDummy = 0;
                foreach (int value in dict.Values) // int 복사
                {
                    localDummy += value;
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_PAIR, result[0], NAME_VALUE, result[1]);
        }

        [ContextMenu("딕셔너리 키 검색 시 단일 키와 튜플 키의 해싱 속도를 비교합니다.")]
        public void TestDictionarySingleVsTupleKey()
        {
            // 준비
            const string NAME_SINGLE = "단일 키 검색";
            const string NAME_TUPLE = "튜플 키 검색";

            int totalLength = _size * _size;
            Dictionary<int, int> singleDict = UCollectionBuilder.CreateDictionary(totalLength, _dummy);
            Dictionary<(int, int), int> tupleDict = UCollectionBuilder.CreateTupleDictionary(totalLength, _dummy);
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_SINGLE, result, 0)) // int 단일 해싱
            {
                int localDummy = 0;
                for (int i = 0; i < totalLength; ++i)
                {
                    localDummy += singleDict[i];
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_TUPLE, result, 1)) // int 튜플 해싱
            {
                int localDummy = 0;
                for (int i = 0; i < totalLength; ++i)
                {
                    localDummy += tupleDict[(i, i)];
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_SINGLE, result[0], NAME_TUPLE, result[1]);
        }
        #endregion
    }
}
