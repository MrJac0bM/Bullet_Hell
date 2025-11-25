using UnityEngine;

[System.Serializable]
public class ShotSetting
{
    [Header("Bullet Properties")]
    public float BulletSpeed = 10f;
    public int NumberOfBullets = 5;
    public Color BulletColor = Color.white;
    
    [Header("Pattern Settings")]
    public ShotPatternType PatternType = ShotPatternType.Radial;
    public float SpreadAngle = 360f; // Para Cone y otros patrones direccionales
    public float WaveAmplitude = 2f; // Para Wave
    public float WaveFrequency = 1f; // Para Wave
    
    [Header("Timing")]
    public float CooldownAfterShoot = 0.1f;
    
    [Header("Rotation")]
    public float RotationSpeed = 15f; // Grados por disparo
    public bool ContinuousRotation = true; // Si continúa rotando o resetea
}