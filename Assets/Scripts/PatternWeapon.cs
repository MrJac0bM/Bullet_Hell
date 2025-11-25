using UnityEngine;
using System.Collections;

public class PatternWeapon : MonoBehaviour
{
    [SerializeField] private ShotPattern shotPattern;
    [SerializeField] private bool autoShoot = true;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private Transform target;
    
    private bool _isShooting = false;
    private float _globalRotationOffset = 0f; 

    private void Start()
    {
        if (shotPattern == null)
        {
            Debug.LogError("¡NO HAY SHOT PATTERN ASIGNADO!");
            return;
        }
        
        if (BulletPool.Instance == null)
        {
            Debug.LogError("¡NO HAY BULLET POOL EN LA ESCENA!");
            return;
        }
        
        Debug.Log($"Pattern configurado: {shotPattern.patternName}");
        Debug.Log($"Settings en pattern: {shotPattern.PatternSettings.Length}");
        
        if (shootOrigin == null) shootOrigin = transform;
        
        if (autoShoot)
        {
            Debug.Log("Iniciando auto-shoot...");
            StartCoroutine(ExecutePattern(shotPattern));
        }
    }

    private void Update()
    {
        if (!_isShooting && UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Disparo manual con ESPACIO");
            StartCoroutine(ExecutePattern(shotPattern));
        }
        
        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
        {
            _globalRotationOffset = 0f;
            Debug.Log("Rotación reseteada");
        }
    }

    private IEnumerator ExecutePattern(ShotPattern pattern)
    {
        _isShooting = true;
        
        Debug.Log($"=== Ejecutando patrón: {pattern.patternName} ===");
        
        for (int lap = 0; lap < pattern.Repetitions; lap++)
        {
            Vector2 center = shootOrigin.position;
            Vector2 aimDirection = GetAimDirection();
            
            Debug.Log($"Lap {lap + 1}/{pattern.Repetitions} - Offset: {_globalRotationOffset:F1}°");
            
            for (int i = 0; i < pattern.PatternSettings.Length; i++)
            {
                ShotSetting setting = pattern.PatternSettings[i];
                
                Debug.Log($"  {setting.PatternType}: {setting.NumberOfBullets} balas a {setting.BulletSpeed} vel");
                
                ShotSystem.ExecutePattern(center, aimDirection, setting, _globalRotationOffset);
                
                if (setting.ContinuousRotation)
                {
                    _globalRotationOffset += setting.RotationSpeed;
                    
                    if (_globalRotationOffset >= 360f)
                        _globalRotationOffset -= 360f;
                    else if (_globalRotationOffset < 0f)
                        _globalRotationOffset += 360f;
                }
                
                yield return new WaitForSeconds(setting.CooldownAfterShoot);
            }
        }

        Debug.Log("=== Patrón completado ===");
        _isShooting = false;
        
        if (autoShoot)
        {
            StartCoroutine(ExecutePattern(shotPattern));
        }
    }
    
    private Vector2 GetAimDirection()
    {
        if (target != null)
        {
            return (target.position - shootOrigin.position).normalized;
        }
        return Vector2.up;
    }
    
    public void ResetRotation()
    {
        _globalRotationOffset = 0f;
    }
    
    public void ChangePattern(ShotPattern newPattern)
    {
        if (_isShooting)
        {
            StopAllCoroutines();
            _isShooting = false;
        }
        
        shotPattern = newPattern;
        _globalRotationOffset = 0f;
        
        if (autoShoot)
        {
            StartCoroutine(ExecutePattern(shotPattern));
        }
    }
}