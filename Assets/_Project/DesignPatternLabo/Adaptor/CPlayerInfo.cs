using UnityEngine;

namespace Project.DesignPatternLabo
{
    public class CPlayerInfo : MonoBehaviour
    {
        [Header("키")]
        [SerializeField] private string _key = "DesignPattern_Adaptor_Player";

        [Header("저장을 시도할 값")]
        [SerializeField] private PlayerSaveData _data;

        private IStorage _storage;


        private void Awake()
        {
            _storage = new PlayerDataAdaptor();
        }

        [ContextMenu("플레이어 데이터 저장하기")]
        public void SavePlayerData()
        {
            PlayerSaveData data = new PlayerSaveData
            {
                name = _data.name,
                level = _data.level,
                exp = _data.exp
            };

            string json = JsonUtility.ToJson(data);
            _storage.Save(_key, json);
        }

        [ContextMenu("플레이어 데이터 불러오기")]
        public void LoadPlayerData()
        {
            string json = _storage.Load(_key);

            if (string.IsNullOrEmpty(json)) return;

            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
            _data.name = data.name;
            _data.level = data.level;
            _data.exp = data.exp;
        }
    }
}
