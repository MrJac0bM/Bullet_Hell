using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Velocity;
    private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private float lifetime = 7f;
    private float _currentLifetime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += (Vector3)(Velocity * Time.deltaTime);

        _currentLifetime -= Time.deltaTime;
        if (_currentLifetime <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        _currentLifetime = lifetime;
    }
    
    public void SetColor(Color color)
    {
        if (_spriteRenderer != null)
        {
            color.a = 1f;
            _spriteRenderer.color = color;
        }
    }
}