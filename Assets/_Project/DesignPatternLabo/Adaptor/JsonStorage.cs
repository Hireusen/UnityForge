using System;
using System.IO;
using UnityEngine;

namespace Project.DesignPatternLabo
{
    public class JsonStorage : IStorage
    {
        public bool Save(string key, string jsonData)
        {
            try
            {
                string fullPath = Path.Combine(Application.persistentDataPath, key + ".json");
                File.WriteAllText(fullPath, jsonData);
                Debug.Log($"[Json Storage] {key}를 저장했습니다.");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Json Storage] {key}를 저장하지 못했습니다.\n{e.Message}");
                return false;
            }
        }

        public string Load(string key)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, key + ".json");

            if (File.Exists(fullPath))
            {
                Debug.Log($"[Json Storage] {key}를 불러왔습니다.");
                return File.ReadAllText(fullPath);
            }

            Debug.LogWarning($"[Json Storage] {key}를 찾을 수 없습니다.");
            return null;
        }
    }
}
