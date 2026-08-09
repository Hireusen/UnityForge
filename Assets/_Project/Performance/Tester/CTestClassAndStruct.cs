using System.Runtime.CompilerServices;
using UnityEngine;
using Project.Default;

namespace Project.Performance
{
    /// <summary>
    /// 구조체와 클래스의 차이를 테스트하는 컴포넌트입니다.
    /// </summary>
    public class CTestClassAndStruct : AMono
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("테스트 값")]
        [SerializeField] private int _size = 10000000;

        private const string NAME_STRUCT = "구조체";
        private const string NAME_CLASS = "클래스";
        private static int _dummy = 0;
        #endregion

        #region ─────────────────────────▷ 컨텍스트 함수 ◁─────────────────────────
        [ContextMenu("멤버 변수 접근과 지역 변수 캐싱의 속도를 비교합니다.")]
        private void TestVariableCachingSpeed()
        {
            // 준비
            const string NAME_MEMBER = "멤버 변수";
            const string NAME_LOCAL = "지역 변수";
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_MEMBER, result, 0)) // 멤버 변수
            {
                int localDummy = 0;
                // 의도적으로 멤버 변수 _size에 매번 접근합니다.
                for (int i = 0; i < _size; ++i)
                {
                    localDummy += _size;
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_LOCAL, result, 1)) // 지역 변수
            {
                int size = _size;
                int localDummy = 0;
                // 캐싱된 지역 변수 size에 접근합니다.
                for (int i = 0; i < size; ++i)
                {
                    localDummy += size;
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_MEMBER, result[0], NAME_LOCAL, result[1]);
        }

        [ContextMenu("구조체와 클래스의 단순 생성 속도를 비교합니다.")]
        private void TestCreateSpeed()
        {
            // 준비
            int size = _size;
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_STRUCT, result, 0)) // 구조체
            {
                NormalStruct[] structs = new NormalStruct[size];
                for (int i = 0; i < size; ++i)
                {
                    structs[i] = new NormalStruct();
                }
            }
            using (new UTimer(NAME_CLASS, result, 1)) // 클래스
            {
                NormalClass[] classes = new NormalClass[size];
                for (int i = 0; i < size; ++i)
                {
                    classes[i] = new NormalClass();
                }
            }

            // 결과 출력
            UTimer.Compare(NAME_STRUCT, result[0], NAME_CLASS, result[1]);
        }

        [ContextMenu("구조체와 클래스의 내부 변수 연산 속도를 비교합니다.")]
        private void TestCalculateSpeed()
        {
            // 준비
            int size = _size;
            var (structs, classes) = CreateData();
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_STRUCT, result, 0)) // 구조체
            {
                int localDummy = 0;
                for (int i = 0; i < size; ++i)
                {
                    localDummy += CalcVariable(structs[i].a, structs[i].b, structs[i].c, structs[i].d);
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_CLASS, result, 1)) // 클래스
            {
                int localDummy = 0;
                for (int i = 0; i < size; ++i)
                {
                    localDummy += CalcVariable(classes[i].a, classes[i].b, classes[i].c, classes[i].d);
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_STRUCT, result[0], NAME_CLASS, result[1]);
        }

        [ContextMenu("구조체와 클래스의 매개 변수 전달 속도를 비교합니다.")]
        private void TestParameterSpeed()
        {
            // 준비
            int size = _size;
            var (structs, classes) = CreateData();
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_STRUCT, result, 0)) // 구조체
            {
                int localDummy = 0;
                for (int i = 0; i < size; ++i)
                {
                    localDummy += TestParameter(structs[i]);
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_CLASS, result, 1)) // 클래스
            {
                int localDummy = 0;
                for (int i = 0; i < size; ++i)
                {
                    localDummy += TestParameter(classes[i]);
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_STRUCT, result[0], NAME_CLASS, result[1]);
        }

        [ContextMenu("구조체의 복사 비용을 비교합니다.")]
        private void TestStructCopySpeed()
        {
            // 준비
            int size = _size;
            const string NAME_STRUCT_COPY = "복사 구조체";
            const string NAME_STRUCT_REF = "Ref 구조체";
            var copyStructs = CreateStructs();
            var refStructs = CreateStructs();
            double[] result = new double[2];

            // 테스트 시작
            using (new UTimer(NAME_STRUCT_COPY, result, 0)) // 복사 구조체
            {
                int localDummy = 0;
                for (int i = 0; i < size; ++i)
                {
                    localDummy += TestParameter(copyStructs[i]);
                }
                _dummy += localDummy;
            }
            using (new UTimer(NAME_STRUCT_REF, result, 1)) // Ref 구조체
            {
                int localDummy = 0;
                for (int i = 0; i < size; ++i)
                {
                    localDummy += TestParameter(ref refStructs[i]);
                }
                _dummy += localDummy;
            }

            // 결과 출력
            UTimer.CompareWithDummy(_dummy, NAME_STRUCT_COPY, result[0], NAME_STRUCT_REF, result[1]);
        }

        [ContextMenu("구조체와 클래스의 박싱 비용을 비교합니다.")]
        private void TestBoxingSpeed()
        {
            // 준비
            int size = _size;
            var (structs, classes) = CreateData();
            double[] result = new double[2];
            object[] objs = new object[size];

            // 테스트 시작
            using (new UTimer(NAME_STRUCT, result, 0)) // 구조체
            {
                for (int i = 0; i < size; ++i)
                {
                    objs[i] = structs[i];
                }
            }
            using (new UTimer(NAME_CLASS, result, 1)) // 클래스
            {
                for (int i = 0; i < size; ++i)
                {
                    objs[i] = classes[i];
                }
            }

            // 결과 출력
            UTimer.Compare(NAME_STRUCT, result[0], NAME_CLASS, result[1]);
        }
        #endregion

        #region ─────────────────────────▷ 내부 메서드 ◁─────────────────────────
        private (NormalStruct[], NormalClass[]) CreateData()
        {
            return (CreateStructs(), CreateClasses());
        }

        private NormalStruct[] CreateStructs()
        {
            int size = _size;
            NormalStruct[] structs = new NormalStruct[size];
            for (int i = 0; i < size; ++i)
            {
                structs[i] = new NormalStruct();
            }
            return structs;
        }

        private NormalClass[] CreateClasses()
        {
            int size = _size;
            NormalClass[] classes = new NormalClass[size];
            for (int i = 0; i < size; ++i)
            {
                classes[i] = new NormalClass();
            }
            return classes;
        }

        private static int CalcVariable(int a, int b, int c, int d)
        {
            return (a * d) - (b * c);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int TestParameter(NormalStruct data)
        {
            return data.a;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int TestParameter(ref NormalStruct data)
        {
            return data.a;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int TestParameter(NormalClass data)
        {
            return data.a;
        }
        #endregion

        #region ─────────────────────────▷ 중첩 타입 ◁─────────────────────────
        private sealed class NormalClass
        {
            public int a, b, c, d;
            public NormalClass(int a = 5)
            {
                this.a = a;
                this.b = 10;
                this.c = 15;
                this.d = 20;
            }
        }

        private struct NormalStruct
        {
            public int a, b, c, d;
            public NormalStruct(int a = 5)
            {
                this.a = a;
                this.b = 10;
                this.c = 15;
                this.d = 20;
            }
        }
        #endregion
    }
}
