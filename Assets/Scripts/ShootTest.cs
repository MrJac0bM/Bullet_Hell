using UnityEngine;

public class ShootTest : MonoBehaviour
{
    [SerializeField] private float _shootCooldown;
    [SerializeField] private RadialShotSetting _radialShootSetting; // ← SIN "h"
    [SerializeField] private float _rotationPerShot = 15f;
    
    private float _shootCooldownTimer = 0f;
    private float _currentRotationOffset = 0f;

    private void Update(){
        _shootCooldownTimer -= Time.deltaTime;
        if(_shootCooldownTimer <= 0f){
            ShotAtack.RadialShot(transform.position, transform.up, _radialShootSetting.BulletSpeed, _radialShootSetting, _currentRotationOffset);
            
            _currentRotationOffset += _rotationPerShot;
            
            if(_currentRotationOffset >= 360f) _currentRotationOffset -= 360f;
            
            _shootCooldownTimer += _radialShootSetting.CooldownAfterShoot;
        }
    }
}