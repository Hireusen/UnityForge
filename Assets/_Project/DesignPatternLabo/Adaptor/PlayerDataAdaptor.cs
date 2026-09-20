using System.Text;
using UnityEngine;

namespace Project.DesignPatternLabo
{
    public class PlayerDataAdaptor : IStorage
    {
        private BinarySDK _storage;

        public PlayerDataAdaptor()
        {
            string path = Application.persistentDataPath;
            _storage = new BinarySDK(path);
        }

        public bool Save(string key, string jsonData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
            return _storage.Write(key, bytes);
        }

        public string Load(string key)
        {
            byte[] bytes = _storage.Read(key);
            if (bytes == null) return null;

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
