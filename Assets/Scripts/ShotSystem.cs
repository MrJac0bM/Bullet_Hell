using UnityEngine;

public static class ShotSystem
{
    public static void Shot(Vector2 origin, Vector2 velocity, Color color)
    {
        Bullet bullet = BulletPool.Instance.RequestBullet();
        bullet.transform.position = origin;
        bullet.Velocity = velocity;
        bullet.SetColor(color);
    }
    
    public static void ExecutePattern(Vector2 origin, Vector2 aimDirection, ShotSetting setting, float rotationOffset = 0f)
    {
        switch (setting.PatternType)
        {
            case ShotPatternType.Radial:
                RadialShot(origin, setting, rotationOffset);
                break;
            case ShotPatternType.Spiral:
                SpiralShot(origin, setting, rotationOffset);
                break;
            case ShotPatternType.Stream:
                StreamShot(origin, aimDirection, setting);
                break;
            case ShotPatternType.Wave:
                WaveShot(origin, aimDirection, setting, rotationOffset);
                break;
            case ShotPatternType.Cross:
                CrossShot(origin, setting, rotationOffset);
                break;
            case ShotPatternType.Star:
                StarShot(origin, setting, rotationOffset);
                break;
            case ShotPatternType.Cone:
                ConeShot(origin, aimDirection, setting, rotationOffset);
                break;
            case ShotPatternType.Ring:
                RingShot(origin, setting, rotationOffset);
                break;
            case ShotPatternType.Flower:
                FlowerShot(origin, setting, rotationOffset);
                break;
            case ShotPatternType.Aimed:
                AimedShot(origin, aimDirection, setting);
                break;
            case ShotPatternType.PlayerShot:
                PlayerShotPattern(origin, aimDirection, setting);
                break;
        }
    }
    
    // RADIAL: Círculo completo de balas
    private static void RadialShot(Vector2 origin, ShotSetting setting, float rotationOffset)
    {
        float angleStep = 360f / setting.NumberOfBullets;
        
        for (int i = 0; i < setting.NumberOfBullets; i++)
        {
            float angle = (angleStep * i + rotationOffset) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Shot(origin, direction * setting.BulletSpeed, setting.BulletColor);
        }
    }
    
    // SPIRAL: Va girando en espiral
    private static void SpiralShot(Vector2 origin, ShotSetting setting, float rotationOffset)
    {
        float angle = rotationOffset * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        
        Shot(origin, direction * setting.BulletSpeed, setting.BulletColor);
    }
    
    // STREAM: Disparo continuo en una dirección
    private static void StreamShot(Vector2 origin, Vector2 aimDirection, ShotSetting setting)
    {
        Shot(origin, aimDirection.normalized * setting.BulletSpeed, setting.BulletColor);
    }
    
    // WAVE: Onda sinusoidal
    private static void WaveShot(Vector2 origin, Vector2 aimDirection, ShotSetting setting, float timeOffset)
    {
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);
        Vector2 perpDirection = new Vector2(-Mathf.Sin(aimAngle), Mathf.Cos(aimAngle));
        
        float waveOffset = Mathf.Sin(timeOffset * setting.WaveFrequency) * setting.WaveAmplitude;
        Vector2 waveOrigin = origin + perpDirection * waveOffset;
        
        // Rota 90 grados la dirección
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        angle += 90f;
        float rotatedAngle = angle * Mathf.Deg2Rad;
        Vector2 rotatedDirection = new Vector2(Mathf.Cos(rotatedAngle), Mathf.Sin(rotatedAngle));
        
        Shot(waveOrigin, rotatedDirection.normalized * setting.BulletSpeed, setting.BulletColor);
    }
    
    // CROSS: Cruz de 4 direcciones
    private static void CrossShot(Vector2 origin, ShotSetting setting, float rotationOffset)
    {
        float[] angles = { 0f, 90f, 180f, 270f };
        
        foreach (float baseAngle in angles)
        {
            float angle = (baseAngle + rotationOffset) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Shot(origin, direction * setting.BulletSpeed, setting.BulletColor);
        }
    }
    
    // STAR: Estrella con puntas largas y cortas
    private static void StarShot(Vector2 origin, ShotSetting setting, float rotationOffset)
    {
        int points = Mathf.Max(3, setting.NumberOfBullets);
        float angleStep = 360f / points;
        
        for (int i = 0; i < points; i++)
        {
            float angle1 = (angleStep * i + rotationOffset) * Mathf.Deg2Rad;
            Vector2 direction1 = new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1));
            Shot(origin, direction1 * setting.BulletSpeed, setting.BulletColor);
            
            float angle2 = (angleStep * i + angleStep / 2f + rotationOffset) * Mathf.Deg2Rad;
            Vector2 direction2 = new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2));
            Shot(origin, direction2 * setting.BulletSpeed * 0.6f, setting.BulletColor);
        }
    }
    
    // CONE: Cono direccional
    private static void ConeShot(Vector2 origin, Vector2 aimDirection, ShotSetting setting, float rotationOffset)
    {
        float baseAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        float angleStep = setting.SpreadAngle / Mathf.Max(1, setting.NumberOfBullets - 1);
        float startAngle = baseAngle - setting.SpreadAngle / 2f;
        
        for (int i = 0; i < setting.NumberOfBullets; i++)
        {
            float angle = (startAngle + angleStep * i + rotationOffset) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Shot(origin, direction * setting.BulletSpeed, setting.BulletColor);
        }
    }
    
    // RING: Anillo completo
    private static void RingShot(Vector2 origin, ShotSetting setting, float rotationOffset)
    {
        float angleStep = 360f / setting.NumberOfBullets;
        
        for (int i = 0; i < setting.NumberOfBullets; i++)
        {
            float angle = (angleStep * i + rotationOffset) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Shot(origin, direction * setting.BulletSpeed, setting.BulletColor);
        }
    }
    
    // FLOWER: Patrón de pétalos
    private static void FlowerShot(Vector2 origin, ShotSetting setting, float rotationOffset)
    {
        int petals = Mathf.Max(3, setting.NumberOfBullets / 3);
        float angleStep = 360f / petals;
        
        for (int i = 0; i < petals; i++)
        {
            float baseAngle = (angleStep * i + rotationOffset) * Mathf.Deg2Rad;
            
            for (int j = -1; j <= 1; j++)
            {
                float angle = baseAngle + (j * 0.3f);
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                
                float speedMultiplier = (j == 0) ? 1.2f : 1.0f;
                Shot(origin, direction * setting.BulletSpeed * speedMultiplier, setting.BulletColor);
            }
        }
    }
    
    // AIMED: Disparo dirigido directamente al jugador
    private static void AimedShot(Vector2 origin, Vector2 targetDirection, ShotSetting setting)
    {
        // Busca al jugador
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector2 directionToPlayer = ((Vector2)player.transform.position - origin).normalized;
            
            // Rota 90 grados a la derecha
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            angle += 90f;
            float rotatedAngle = angle * Mathf.Deg2Rad;
            Vector2 rotatedDirection = new Vector2(Mathf.Cos(rotatedAngle), Mathf.Sin(rotatedAngle));
            
            Shot(origin, rotatedDirection * setting.BulletSpeed, setting.BulletColor);
        }
    }

    // PLAYER SHOT: Disparo del jugador hacia el jefe (90 grados hacia arriba)
    private static void PlayerShotPattern(Vector2 origin, Vector2 aimDirection, ShotSetting setting)
    {
        // Rota -90 grados (hacia arriba)
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        angle += 0f;
        float rotatedAngle = angle * Mathf.Deg2Rad;
        Vector2 rotatedDirection = new Vector2(Mathf.Cos(rotatedAngle), Mathf.Sin(rotatedAngle));
        
        Shot(origin, rotatedDirection.normalized * setting.BulletSpeed, setting.BulletColor);
    }
}