using System.Runtime.CompilerServices;

/// <summary>
/// 문자열 확장 메서드를 담는 유틸리티
/// </summary>
public static class StringExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBlank(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotBlank(this string str)
    {
        return !IsBlank(str);
    }
}
