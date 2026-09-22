using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.AddressableLabo
{
    public class CreateAddressablePrefab : MonoBehaviour
    {
        [Header("어드레서블 주소")]
        [SerializeField] private AssetReferenceGameObject _ref;
        [SerializeField] private AssetReferenceT<AudioClip> _ref2;
        [SerializeField] private AssetReferenceSprite _ref3;
        [SerializeField] private AssetLabelReference _ref4;
        [SerializeField] private int _count;

        [Header("생성 규칙")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _parent;

        [Header("실행 키")]
        [SerializeField] private KeyCode _createkey = KeyCode.Tab;
        [SerializeField] private KeyCode _cleanKey = KeyCode.Tab;

        private List<GameObject> _loadedGO = new();

        private void Update()
        {
            if (Input.GetKeyDown(_createkey))
            {
                SpawnPrefab();
            }
            else if (Input.GetKeyDown(_cleanKey))
            {
                CleanPrefab();
            }
        }

        private async void SpawnPrefab()
        {
            if (_ref == null || !_ref.RuntimeKeyIsValid()) return;

            // 오브젝트 모두 생성하기
            for(int i = 0; i < _count; ++i)
            {
                AsyncOperationHandle<GameObject> handle =
                    _ref.InstantiateAsync(_spawnPoint.position, Quaternion.identity, _parent); // 핸들 가져오기
                GameObject go = await handle.Task; // 비동기 생성 대기

                if(handle.Status != AsyncOperationStatus.Succeeded) continue; // 정상 생성 확인

                _loadedGO.Add(go);
            }
        }

        private void CleanPrefab()
        {
            for (int i = 0; i < _loadedGO.Count; ++i)
            {
                GameObject go = _loadedGO[i];
                if (go == null) continue;

                Addressables.ReleaseInstance(go);
            }

            _loadedGO.Clear();
        }

        private void OnDestroy()
        {
            CleanPrefab();
        }
    }
}
