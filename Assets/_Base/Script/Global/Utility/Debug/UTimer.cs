using System;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 성능 측정과 로그 출력을 담당하는 구조체입니다.
/// using(new UTimer("이름")) { ... } 형태로 사용하여 코드 블록의 실행 시간을 측정합니다.
/// </summary>
public readonly struct UTimer : IDisposable
{
    private readonly string _name;
    private readonly long _startTime;
    private readonly double[] _result;

    /// <summary>
    /// 생성자 : 타이머를 시작합니다.
    /// 결과값을 저장할 배열을 전달할 수 있습니다. (0번 인덱스만 사용)
    /// </summary>
    public UTimer(string name, double[] result = null)
    {
        _name = name;
        _startTime = Stopwatch.GetTimestamp();
        _result = result;
    }

    public void Dispose()
    {
        long endTime = Stopwatch.GetTimestamp();
        double elapsedSeconds = (double)(endTime - _startTime) / Stopwatch.Frequency;
        if (_result != null && _result.Length > 0) {
            _result[0] = elapsedSeconds;
        }
        UDebug.Print($"{_name}의 종료까지 {elapsedSeconds}초 걸렸습니다.");
    }

    public static void Compare(string nameA, double elapsedSecondsA, string nameB, double elapsedSecondsB)
    {
        if (elapsedSecondsA < elapsedSecondsB) {
            UDebug.Print($"{nameA}가 {nameB}보다 {elapsedSecondsB - elapsedSecondsA}초 빠릅니다.");
        }
        else if (elapsedSecondsA > elapsedSecondsB) {
            UDebug.Print($"{nameB}가 {nameA}보다 {elapsedSecondsA - elapsedSecondsB}초 빠릅니다.");
        }
        else {
            UDebug.Print($"{nameA}와 {nameB}는 동일한 시간을 사용했습니다. ({elapsedSecondsA}초)");
        }
    }
}
