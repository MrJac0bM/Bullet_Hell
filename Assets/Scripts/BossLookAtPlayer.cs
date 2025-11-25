using UnityEngine;

public class BossLookAtPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float padding = 1f; // Distancia desde los bordes
    
    private Vector2 _moveDirection;
    private float _changeDirectionTimer = 0f;
    private float _changeDirectionInterval = 3f; // Cada 3 segundos cambia de dirección

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        ChooseNewDirection();
    }

    private void Update()
    {
        if (player == null) return;

        LookAtPlayer();
        MoveRandomly();
    }

    private void LookAtPlayer()
    {
        // Calcula la dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;
        
        // Calcula el ángulo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rota 90 grados hacia abajo para que mire de frente
        angle += 90f;
        
        // Rota suavemente hacia el jugador
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void MoveRandomly()
    {
        // Cambia de dirección periódicamente
        _changeDirectionTimer -= Time.deltaTime;
        if (_changeDirectionTimer <= 0f)
        {
            ChooseNewDirection();
            _changeDirectionTimer = _changeDirectionInterval;
        }

        // Mueve el jefe
        transform.position += (Vector3)_moveDirection * moveSpeed * Time.deltaTime;

        // Mantiene el jefe dentro de los límites de la pantalla
        Vector3 pos = transform.position;
        Vector3 screenMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 screenMax = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        pos.x = Mathf.Clamp(pos.x, screenMin.x + padding, screenMax.x - padding);
        pos.y = Mathf.Clamp(pos.y, screenMin.y + padding, screenMax.y - padding);
        
        transform.position = pos;
    }

    private void ChooseNewDirection()
    {
        // Elige una dirección aleatoria
        float randomAngle = Random.Range(0f, 360f);
        _moveDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 
                                    Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
    }
}