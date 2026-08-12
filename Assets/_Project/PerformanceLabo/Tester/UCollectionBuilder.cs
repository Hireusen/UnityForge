using System.Collections.Generic;

namespace Project.Performance
{
    /// <summary>
    /// 벤치마크 테스트에 필요한 컬렉션 데이터를 생성하는 유틸리티 클래스입니다.
    /// </summary>
    public static class UCollectionBuilder
    {
        public static int[,] Create2DArray(int size)
        {
            int[,] arr = new int[size, size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    arr[y, x] = 1;
                }
            }
            return arr;
        }

        public static int[] Create1DArray(int totalSize)
        {
            int[] arr = new int[totalSize];
            for (int i = 0; i < totalSize; i++)
            {
                arr[i] = 1;
            }
            return arr;
        }

        public static int[][] CreateJaggedArray(int size)
        {
            int[][] arr = new int[size][];
            for (int y = 0; y < size; y++)
            {
                arr[y] = new int[size];
                for (int x = 0; x < size; x++)
                {
                    arr[y][x] = 1;
                }
            }
            return arr;
        }

        public static List<int> CreateList(int totalSize)
        {
            List<int> list = new List<int>(totalSize);
            for (int i = 0; i < totalSize; i++)
            {
                list.Add(1);
            }
            return list;
        }

        public static Dictionary<int, int> CreateDictionary(int totalSize, int fillValue)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>(totalSize);
            for (int i = 0; i < totalSize; ++i)
            {
                dict.Add(i, fillValue);
            }
            return dict;
        }

        public static Dictionary<(int, int), int> CreateTupleDictionary(int totalSize, int fillValue)
        {
            Dictionary<(int, int), int> dict = new Dictionary<(int, int), int>(totalSize);
            for (int i = 0; i < totalSize; ++i)
            {
                dict.Add((i, i), fillValue);
            }
            return dict;
        }
    }
}
