using System.Collections.Generic;
using UnityEngine;

namespace Project.BulletArt
{
    public class CProjectileManager : MonoBehaviour
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("초기값")]
        [SerializeField] private int _initialProjectileCount = 1024;

        private List<ProjectileData> _datas;
        private List<int> _removeQueue;
        private int _count;

        private CProjectileRenderer _renderer = new();
        #endregion

        #region ─────────────────────────▷ 메세지 함수 ◁─────────────────────────
        private void Awake()
        {
            _initialProjectileCount = Mathf.Max(1, _initialProjectileCount);
            _datas = new(_initialProjectileCount);
        }

        private void Update()
        {
            _move.Execute(_datas);
            _collsion.Execute(_datas);
            ProjectileRemove.Execute(_datas, _removeQueue, _count);
        }

        private void LateUpdate()
        {
            _renderer.Execute();
        }
        #endregion
    }
}
