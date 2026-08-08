using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// 배열과 리스트를 다루는 유틸리티 클래스입니다.
/// </summary>
public static class ArrayExtension
{
    /// <summary>
    /// 피셔 예이츠 셔플로 배열을 무작위로 섞습니다.
    /// </summary>
    public static void Shuffle<T>(this T[] array)
    {
        if (array == null) return;

        int length = array.Length - 1;
        for (int i = length; i > 0; --i)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }

    /// <summary>
    /// 피셔 예이츠 셔플로 리스트를 무작위로 섞습니다.
    /// </summary>
    public static void Shuffle<T>(this List<T> list)
    {
        if (list == null) return;

        int length = list.Count - 1;
        for (int i = length; i > 0; --i)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }

    /// <summary>
    /// 리스트의 마지막 요소와 교체하고, 마지막 요소를 삭제합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SwapLastAndRemove<T>(this List<T> list, int index)
    {
        if (list == null) return;

        int last = list.Count - 1;
        if (index < 0 || last < index) return;

        list[index] = list[last];
        list.RemoveAt(last);
    }
}
