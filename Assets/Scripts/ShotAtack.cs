using UnityEngine;

public static class ShotAtack
{
    public static void Shot(Vector2 origin, Vector2 Velocity){
        Bullet bullet = BulletPool.Instance.RequestBullet();
        bullet.transform.position = origin;
        bullet.Velocity = Velocity;
    }

    // ← AÑADIR parámetro rotationOffset
   public static void RadialShot(Vector2 origin, Vector2 aimDirection, float bulletSpeed, RadialShotSetting radialShotSetting, float rotationOffset = 0f) // ← SIN "h"
{
    float angleStep = 360f / radialShotSetting.NumberOfBullets;
    float angle = rotationOffset;

    for (int i = 0; i < radialShotSetting.NumberOfBullets; i++)
    {
        float bulletDirXPosition = origin.x + Mathf.Sin((angle * Mathf.PI) / 180);
        float bulletDirYPosition = origin.y + Mathf.Cos((angle * Mathf.PI) / 180);

        Vector2 bulletVector = new Vector2(bulletDirXPosition, bulletDirYPosition);
        Vector2 bulletMoveDirection = (bulletVector - origin).normalized;

        Shot(origin, bulletMoveDirection * bulletSpeed);

        angle += angleStep;
    }
}
}