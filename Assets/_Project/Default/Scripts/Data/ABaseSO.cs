using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Project.Default
{
    /// <summary>
    /// 모든 데이터 SO의 기반 클래스
    /// </summary>
    public abstract class ABaseSO : ScriptableObject
    {
        #region ─────────────────────────▷ 내부 멤버 ◁─────────────────────────
        [SerializeField] protected int _id = -1;
        #endregion

        #region ─────────────────────────▷ 공개 멤버 ◁─────────────────────────
        public int Id => _id;
        #endregion

        #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
        /// <summary>
        /// 오버라이드하여 사용해주세요. 값 유효성을 검사합니다.
        /// </summary>
        protected virtual void CollectErrorMessage(List<string> errorList)
        {
            if (_id < 0) errorList.Add($"{errorList.Count + 1}. ID가 비어있습니다.");
        }

        /// <summary>
        /// 배열이 할당되지 않았거나 기본값이 들어있는 경우 에러 목록에 추가합니다.
        /// </summary>
        protected static void IncorrectArrayToAddError<T>(T[] array, T defaultValue, List<string> errorList) where T : struct
        {
            if (array == null || array.Length == 0)
            {
                errorList.Add($"{errorList.Count + 1}. {typeof(T).Name} 배열이 비어있거나 길이가 0입니다.");
            }
            else
            {
                int length = array.Length;
                for (int i = 0; i < length; ++i)
                {
                    if (!EqualityComparer<T>.Default.Equals(array[i], defaultValue)) continue;
                    errorList.Add($"{errorList.Count + 1}. {typeof(T).Name} 배열의 {i}번째 인덱스에 올바른 값이 할당되지 않았습니다.");
                }
            }
        }
        #endregion

        #region ─────────────────────────▷ 메시지 함수 ◁─────────────────────────
        protected virtual void OnValidate()
        {
            List<string> errorList = new();
            CollectErrorMessage(errorList);
            if (errorList.Count > 0)
            {
                StringBuilder sb = new();
                sb.AppendLine($"SO 인스턴스({this.name})의 값이 올바르지 않습니다.");
                int length = errorList.Count;
                for (int i = 0; i < length; ++i)
                {
                    sb.AppendLine(errorList[i]);
                }
                UDebug.PrintOnce(sb, LogType.Warning);
            }
        }
        #endregion
    }

}
