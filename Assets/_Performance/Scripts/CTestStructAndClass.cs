using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 구조체와 클래스의 차이를 테스트하는 컴포넌트입니다.
/// </summary>
public class CTestStructAndClass : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("테스트 값")]
    [SerializeField] private int _size = 10000000;
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private const string NAME_STRUCT = "구조체";
    private const string NAME_CLASS = "클래스";
    private static int _dummy = 0;
    #endregion

    #region ─────────────────────────▶ 컨텍스트 함수 ◀─────────────────────────
    [ContextMenu("구조체와 클래스의 단순 생성 속도를 비교합니다.")]
    private void TestCreateSpeed()
    {
        // 준비
        double[] result = new double[2];
        // 테스트 시작
        using (new UTimer(NAME_STRUCT, result, 0)) {
            NormalStruct[] structs = new NormalStruct[_size];
            for (int i = 0; i < _size; ++i) {
                structs[i] = new NormalStruct();
            }
        }
        using (new UTimer(NAME_CLASS, result, 1)) {
            NormalClass[] classes = new NormalClass[_size];
            for (int i = 0; i < _size; ++i) {
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
        var (structs, classes) = CreateData();
        double[] result = new double[2];
        // 테스트 시작
        using (new UTimer(NAME_STRUCT, result, 0)) {
            for (int i = 0; i < _size; ++i) {
                CalcVariable(structs[i].a, structs[i].b, structs[i].c, structs[i].d);
            }
        }
        using (new UTimer(NAME_CLASS, result, 1)) {
            for (int i = 0; i < _size; ++i) {
                CalcVariable(classes[i].a, classes[i].b, classes[i].c, classes[i].d);
            }
        }
        // 결과 출력
        UTimer.Compare(NAME_STRUCT, result[0], NAME_CLASS, result[1]);
    }

    [ContextMenu("구조체와 클래스의 매개 변수 전달 속도를 비교합니다.")]
    private void TestParameterSpeed()
    {
        // 준비
        var (structs, classes) = CreateData();
        double[] result = new double[2];
        // 테스트 시작
        using (new UTimer(NAME_STRUCT, result, 0)) {
            for (int i = 0; i < _size; ++i) {
                TestParameter(structs[i]);
            }
        }
        using (new UTimer(NAME_CLASS, result, 1)) {
            for (int i = 0; i < _size; ++i) {
                TestParameter(classes[i]);
            }
        }
        // 결과 출력
        UTimer.Compare(NAME_STRUCT, result[0], NAME_CLASS, result[1]);
    }

    [ContextMenu("구조체의 복사 비용을 비교합니다.")]
    private void TestStructCopySpeed()
    {
        // 준비
        const string NAME_STRUCT_COPY = "복사 구조체";
        const string NAME_STRUCT_REF = "Ref 구조체";
        var copyStructs = CreateStructs();
        var refStructs = CreateStructs();
        double[] result = new double[2];
        // 테스트 시작
        using (new UTimer(NAME_STRUCT_COPY, result, 0)) {
            for (int i = 0; i < _size; ++i) {
                TestParameter(copyStructs[i]);
            }
        }
        using (new UTimer(NAME_STRUCT_REF, result, 1)) {
            for (int i = 0; i < _size; ++i) {
                TestParameter(ref refStructs[i]);
            }
        }
        // 결과 출력
        UTimer.Compare(NAME_STRUCT_COPY, result[0], NAME_STRUCT_REF, result[1]);
    }

    [ContextMenu("구조체와 클래스의 박싱 비용을 비교합니다.")]
    private void TestBoxingSpeed()
    {
        // 준비
        var (structs, classes) = CreateData();
        double[] result = new double[2];
        object[] objs = new object[_size];
        // 테스트 시작
        using (new UTimer(NAME_STRUCT, result, 0)) {
            for (int i = 0; i < _size; ++i) {
                objs[i] = structs[i];
            }
        }
        using (new UTimer(NAME_CLASS, result, 1)) {
            for (int i = 0; i < _size; ++i) {
                objs[i] = classes[i];
            }
        }
        // 결과 출력
        UTimer.Compare(NAME_STRUCT, result[0], NAME_CLASS, result[1]);
    }
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private (NormalStruct[], NormalClass[]) CreateData()
    {
        return (CreateStructs(), CreateClasses());
    }

    private NormalStruct[] CreateStructs()
    {
        NormalStruct[] structs = new NormalStruct[_size];
        for (int i = 0; i < _size; ++i) {
            structs[i] = new NormalStruct();
        }
        return structs;
    }

    private NormalClass[] CreateClasses()
    {
        NormalClass[] classes = new NormalClass[_size];
        for (int i = 0; i < _size; ++i) {
            classes[i] = new NormalClass();
        }
        return classes;
    }

    private static int CalcVariable(int a, int b, int c, int d)
    {
        return (a * d) - (b * c);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void TestParameter(NormalStruct data)
    {
        _dummy += data.a;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void TestParameter(ref NormalStruct data)
    {
        _dummy += data.a;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void TestParameter(NormalClass data)
    {
        _dummy += data.a;
    }
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
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
