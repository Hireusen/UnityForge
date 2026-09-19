using UnityEngine;

namespace Project.DesignPatternLabo
{
    public class CMoveObject : MonoBehaviour
    {
        [Header("이동 거리")]
        [SerializeField] private float _moveStep = 1f;

        public void MoveForward() => Move(Vector3.forward);
        public void MoveBack() => Move(Vector3.back);
        public void MoveLeft() => Move(Vector3.left);
        public void MoveRight() => Move(Vector3.right);

        public void Move(Vector3 dir)
        {
            transform.Translate(_moveStep * dir);
        }
    }
}
