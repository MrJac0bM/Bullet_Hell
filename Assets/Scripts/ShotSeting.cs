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
    public float SpreadAngle = 360f; 
    public float WaveAmplitude = 2f;
    public float WaveFrequency = 1f; 
    
    [Header("Timing")]
    public float CooldownAfterShoot = 0.1f;
    
    [Header("Rotation")]
    public float RotationSpeed = 15f; 
    public bool ContinuousRotation = true; 
}