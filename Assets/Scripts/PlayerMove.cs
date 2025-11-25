using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float slowdownMultiplier = 0.5f; // 50% más lento
    
    private Vector2 _velocity = Vector2.zero;

    [SerializeField] private Transform boss;
    [SerializeField] private ShotPattern shootPattern;
    [SerializeField] private float shootCooldown = 0.5f;
    
    private Vector2 _input = Vector2.zero;
    private float _shootTimer = 0f;
    
    private void Update()
    {
        UpdatePlayer();
        LookAtBoss();
        HandleShooting();
    }

    private void LookAtBoss()
    {
        if (boss == null) return;

        Vector2 direction = (boss.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rota 90 grados hacia arriba
        angle += 90f;
        
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    private void UpdatePlayer()
    {
        HandleInputPlayer();
        UpdatePosition();
    }

    private void HandleInputPlayer()
    {
        _input = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            _input.y += 1;
        if (Input.GetKey(KeyCode.S))
            _input.y -= 1;
        if (Input.GetKey(KeyCode.A))
            _input.x -= 1;
        if (Input.GetKey(KeyCode.D))
            _input.x += 1;

        // Si presiona Shift, ralentiza TODO (balas, jefe, etc)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Time.timeScale = slowdownMultiplier;
        }
        else
        {
            Time.timeScale = 1f;
        }

        _velocity = _input.normalized * speed;
    }

    private void UpdatePosition()
    {
        transform.position += (Vector3)_velocity * Time.deltaTime;
    }

    private void HandleShooting()
    {
        _shootTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.X) && _shootTimer <= 0f && shootPattern != null && boss != null)
        {
            Vector2 center = transform.position;
            Vector2 direction = (boss.position - transform.position).normalized;

            for (int i = 0; i < shootPattern.PatternSettings.Length; i++)
            {
                ShotSetting setting = shootPattern.PatternSettings[i];
                ShotSystem.ExecutePattern(center, direction, setting, 0f);
            }

            _shootTimer = shootCooldown;
        }
    }
}