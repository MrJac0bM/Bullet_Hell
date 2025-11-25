using UnityEngine;
using System.Collections;

public class RadialShotWeapon : MonoBehaviour
{
   [SerializeField] private RadialShotPatern shotPatern;
   [SerializeField] private bool autoShoot = true;
   [SerializeField] private Transform shootOrigin; // ← NUEVO: punto de disparo compartido
   private bool _isShooting = false;

   private void Start()
   {
        // Si no hay origen asignado, usar la posición de este objeto
        if (shootOrigin == null)
        {
            shootOrigin = transform;
        }
        
        if (autoShoot)
        {
            StartCoroutine(ExecuteRadialShotPatern(shotPatern));
        }
   }

   private void Update()
   {
        if (!_isShooting && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ExecuteRadialShotPatern(shotPatern));
        }
   }

   private IEnumerator ExecuteRadialShotPatern(RadialShotPatern patern)
   {
        _isShooting = true;

        Vector2 aimDirection = transform.up;
        float rotationOffset = 0f;

        for (int lap = 0; lap < patern.Repetitions; lap++)
        {
            Vector2 center = shootOrigin.position; // ← Usar el origen compartido
            
            for (int i = 0; i < patern.PatternSettings.Length; i++)
            {
                RadialShotSetting setting = patern.PatternSettings[i];
                
                ShotAtack.RadialShot(center, aimDirection, setting.BulletSpeed, setting, rotationOffset);
                
                rotationOffset += 15f;
                if (rotationOffset >= 360f) rotationOffset -= 360f;
                
                yield return new WaitForSeconds(setting.CooldownAfterShoot);
            }
        }

        _isShooting = false;
        
        if (autoShoot)
        {
            StartCoroutine(ExecuteRadialShotPatern(shotPatern));
        }
   }
}