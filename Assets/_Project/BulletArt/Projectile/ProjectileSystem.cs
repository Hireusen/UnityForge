using System.Collections.Generic;
using UnityEngine;

namespace Project.BulletArt
{
    public static class ProjectileSystem
    {
        public static void CheckLifeTime(List<ProjectileData> datas, int count)
        {
            float currentTime = Time.time;
            float deltaTime = Time.deltaTime;

            for (int i = count - 1; i >= 0; --i) {
                // 수명 연산
                var bullet = datas[i];
                if (bullet.removeTime < currentTime) {
                    datas[i] = datas[count - 1];
                    count--;
                }

                var so = bullet.so;
                // 이동 연산
                bullet.currentSpeed += so.Acceleration * deltaTime;
                bullet.currentSpeed = Mathf.Clamp(bullet.currentSpeed, so.ClampSpeed.x, so.ClampSpeed.y);
                float rad = bullet.currentAngle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
                bullet.position += direction * (bullet.currentSpeed * deltaTime);

                // 충돌 연산
                float distSquare = (bullet.position)
            }
        }

        public static int Execute(List<ProjectileData> datas, int count)
        {
            for(int i = 0; i < removeQueue.Count; ++i) {
                int targetIndex = removeQueue[i];
                datas[targetIndex] = datas[count - 1];
                count--;
            }

            return count;
        }

        private static void RemoveProjectile(List<ProjectileData> datas, int index, ref int count)
        {
            datas[index] = datas[count - 1];
            count--;
        }
    }
}
