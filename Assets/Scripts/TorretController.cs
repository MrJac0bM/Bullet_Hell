using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Disparo")]
    [SerializeField] private ShotPattern shotPattern;
    [SerializeField] private float shootCooldown = 0.5f;
    
    private float _shootTimer = 0f;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        if (shotPattern == null)
        {
            Debug.LogError("¡Asigna un ShotPattern a la torreta!");
        }
    }

    private void Update()
    {
        if (player == null) return;
        
        LookAtPlayer();
        Shoot();
    }

    private void LookAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rota 90 grados hacia arriba
        angle -= 90f;
        
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        _shootTimer -= Time.deltaTime;
        
        if (_shootTimer <= 0f && shotPattern != null)
        {
            Vector2 center = transform.position;
            float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            for (int i = 0; i < shotPattern.PatternSettings.Length; i++)
            {
                ShotSetting setting = shotPattern.PatternSettings[i];
                ShotSystem.ExecutePattern(center, direction, setting, 0f);
            }

            _shootTimer = shootCooldown;
        }
    }
}