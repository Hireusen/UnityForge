using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// 배열과 리스트를 다루는 유틸리티 클래스입니다.
/// </summary>
public static class UArray
{
    /// <summary>
    /// 배열의 크기를 원하는 배율로 확장합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetArray"></param>
    /// <param name="multiplySize">확장 배율</param>
    /// <returns></returns>
    public static bool TryResizeArray<T>(ref T[] targetArray, double multiplySize)
    {
        const int MAX_ARRAY_SIZE = int.MaxValue - 56;

        // 초기 방어
        if (targetArray == null) return false;
        if (multiplySize <= 0) return false;
        if (double.IsNaN(multiplySize)) return false;
        if (double.IsInfinity(multiplySize)) return false;
        if (targetArray.Length >= MAX_ARRAY_SIZE) return false;

        // 목표 크기 설정
        long newSize = (long)Math.Ceiling(targetArray.Length * multiplySize);
        if(multiplySize > 1d && newSize <= targetArray.Length)
        {
            newSize = targetArray.Length + 1; // 최소 1 증가
        }

        if (newSize <= 0) return false; // 목표 크기가 0 이하
        newSize = Math.Min(newSize, MAX_ARRAY_SIZE); // 최대치 제한

        // 배열 확장 시도
        try
        {
            Array.Resize(ref targetArray, (int)newSize);
            return true;
        }
        catch (OutOfMemoryException) // 메모리 부족
        { 
            return false;
        }
    }

    /// <summary>
    /// 리스트가 완전히 초기화된 상태인지 검사합니다.
    /// </summary>
    public static bool IsInitedList<T>(List<T> list)
    {
        // 내부 값 없나?
        if (list == null) return false;
        int count = list.Count;
        if (count <= 0) return false;

        // 값 형식인가?
        if (typeof(T).IsValueType) return true;
        // 참조 형식인가?
        for (int i = 0; i < count; ++i)
        {
            if (list[i] == null) return false;
        }

        return true;
    }

    /// <summary>
    /// 1차원 배열이 완전히 초기화된 상태인지 검사합니다.
    /// </summary>
    public static bool IsInitedArray<T>(T[] array)
    {
        // 내부 값 없나?
        if (array == null) return false;
        int length = array.Length;
        if (length <= 0) return false;

        // 값 형식인가?
        if (typeof(T).IsValueType) return true;
        // 참조 형식인가?
        for (int i = 0; i < length; ++i)
        {
            if (array[i] == null) return false;
        }

        return true;
    }

    /// <summary>
    /// 인덱스가 배열 범위 안에 있는지 검사합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InBounds<T>(T[] array, int index)
    {
        // index가 음수일 경우 오버플로우 유도
        return (array != null) && ((uint)index < (uint)array.Length);
    }

    /// <summary>
    /// 인덱스가 리스트 범위 안에 있는지 검사합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InBounds<T>(List<T> list, int index)
    {
        // index가 음수일 경우 오버플로우 유도
        return (list != null) && ((uint)index < (uint)list.Count);
    }
}
