using UnityEngine;

namespace Project.BulletArt
{
    public struct ProjectileData
    {
        public ProjectileSO so;
        public Vector2 targetPos;
        public float targetIndex;
        public Vector2 position;
        public float currentSpeed;
        public float currentAngle;
        public float removeTime;

        public ProjectileData(ProjectileSO so, Vector2 startPos, float startAngle, float startSpeed, float removeTime)
        {
            this.so = so;
            this.position = startPos;
            this.currentAngle = startAngle;
            this.currentSpeed = startSpeed;
            this.removeTime = removeTime;
        }
    }
}
