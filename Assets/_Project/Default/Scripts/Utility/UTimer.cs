using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace Project.Default
{
    /// <summary>
    /// 성능 측정과 로그 출력을 담당하는 구조체입니다.
    /// using(new UTimer("이름")) { ... } 형태로 사용하여 코드 블록의 실행 시간을 측정합니다.
    /// </summary>
    public readonly struct UTimer : IDisposable
    {
        private readonly string _name;
        private readonly long _startTime;
        private readonly double[] _result;
        private readonly int _index;

        /// <summary>
        /// 벤치마크 결과를 받을 수 있는 이벤트
        /// </summary>
        public static event Action<string> OnBenchmarkComplete;

        /// <summary>
        /// 생성자 : 타이머를 시작합니다.
        /// 결과값을 저장할 배열을 전달할 수 있습니다.
        /// 저장할 인덱스를 지정하지 않으면 0번 인덱스를 사용합니다.
        /// </summary>
        public UTimer(string name, double[] result = null, int index = 0)
        {
            _name = name;
            _startTime = Stopwatch.GetTimestamp(); // 하드웨어 진동 횟수
            _result = result;
            _index = Mathf.Clamp(index, 0, int.MaxValue);
        }

        public void Dispose()
        {
            long endTime = Stopwatch.GetTimestamp();
            double elapsedSeconds = (double)(endTime - _startTime) / Stopwatch.Frequency;

            if (_result != null && _result.Length > _index)
            {
                _result[_index] = elapsedSeconds;
            }

            UDebug.Print($"{_name}의 종료까지 {elapsedSeconds:F4}초 걸렸습니다.");
        }

        public static void Compare(string nameA, double elapsedSecondsA, string nameB, double elapsedSecondsB)
        {
            const string WINNER_COLOR = "<color=#1E90FF>";
            const string LOSER_COLOR = "<color=#FF4500>";
            const string RATIO_COLOR = "<color=#FFD700>";

            StringBuilder sb = new();

            // 결과 메시지 조립
            if (elapsedSecondsA < elapsedSecondsB)
            {
                sb.Append($"{nameA}(이)가 {nameB}보다 {(elapsedSecondsB / elapsedSecondsA):F2}배 빠릅니다.");
            }
            else if (elapsedSecondsA > elapsedSecondsB)
            {
                sb.Append($"{nameB}(이)가 {nameA}보다 {(elapsedSecondsA / elapsedSecondsB):F2}배 빠릅니다.");
            }
            else
            {
                sb.Append($"{nameA}와(과) {nameB}(은)는 동일한 시간을 사용했습니다. ({elapsedSecondsA:F3}초)");
            }

            // 상세 메시지 조립
            string ratio = $"{RATIO_COLOR}{((elapsedSecondsA > elapsedSecondsB) ? (elapsedSecondsA / elapsedSecondsB) : (elapsedSecondsB / elapsedSecondsA)):F2}배</color>";
            string winner = $"{WINNER_COLOR}{((elapsedSecondsA > elapsedSecondsB) ? nameB : nameA)}</color>";
            string loser = $"{LOSER_COLOR}{((elapsedSecondsA > elapsedSecondsB) ? nameA : nameB)}</color>";

            double winTime = (elapsedSecondsA > elapsedSecondsB ? elapsedSecondsB : elapsedSecondsA);
            double loseTime = (elapsedSecondsA > elapsedSecondsB ? elapsedSecondsA : elapsedSecondsB);
            sb.AppendLine($"({winner}: {winTime:F3}ms / {loser}: {loseTime:F3}ms)");

            // 결과 출력
            string message = sb.ToString();
            UDebug.Print(message);
            UDebug.Print("── ── ── ── ── ── ── ── ── ── ──");

            OnBenchmarkComplete?.Invoke(message);
        }

        public static void CompareWithDummy<T>(T dummy, string nameA, double elapsedSecondsA, string nameB, double elapsedSecondsB)
        {
            if (EqualityComparer<T>.Default.Equals(dummy))
            {
                string message = "이런 우연이? 테스트를 다시 시도해주세요!";
                UDebug.Print(message);
                OnBenchmarkComplete?.Invoke(message);
            }
            else
            {
                Compare(nameA, elapsedSecondsA, nameB, elapsedSecondsB);
            }
        }
    }
}
