using UnityEngine;
using System.Collections.Generic;

namespace Project.BulletArt
{
    public class CProjectileRenderer
    {
        #region ─────────────────────────▷ 내부 변수 ◁─────────────────────────
        [Header("참조 연결")]
        [SerializeField] private Mesh _instanceMesh;
        [SerializeField] private Material _instanceMaterial;

        private List<Matrix4x4[]> _matrixBatches = new();
        #endregion

        #region ─────────────────────────▷ 공개 멤버 ◁─────────────────────────
        public void AddMatrix(in Matrix4x4[] batch)
        {
            _matrixBatches.Add(batch);
        }

        public void Execute()
        {
            if (_instanceMesh == null || _instanceMaterial == null) return;

            var count = _matrixBatches.Count;
            for (int i = 0; i < count; ++i) {
                Matrix4x4[] batch = _matrixBatches[i];
                Graphics.DrawMeshInstanced(_instanceMesh, 0, _instanceMaterial, batch, batch.Length);
            }
            _matrixBatches.Clear();
        }
        #endregion
    }
}
