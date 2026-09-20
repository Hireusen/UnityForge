using System;
using System.IO;
using UnityEngine;

namespace Project.DesignPatternLabo
{
    public sealed class BinarySDK
    {
        private string _savePath;

        public BinarySDK(string savePath)
        {
            _savePath = savePath;
        }

        public bool Write(string fileName, byte[] data)
        {
            try
            {
                string fullPath = Path.Combine(_savePath, fileName + ".bin");
                File.WriteAllBytes(fullPath, data);
                Debug.Log($"[Binary SDK] {fileName}을 저장했습니다.");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Binary SDK] {fileName}을 저장하지 못했습니다.\n{e.Message}");
                return false;
            }
        }

        public byte[] Read(string fileName)
        {
            string fullPath = Path.Combine(_savePath, fileName + ".bin");

            if (File.Exists(fullPath))
            {
                Debug.Log($"[Binary SDK] {fileName}을 불러왔습니다.");
                return File.ReadAllBytes(fullPath);
            }

            Debug.LogWarning($"[Binary SDK] {fileName}을 찾을 수 없습니다.");
            return null;
        }
    }
}
