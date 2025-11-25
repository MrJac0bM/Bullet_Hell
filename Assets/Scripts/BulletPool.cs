using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance;
    public static BulletPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BulletPool>();
            }
            return _instance;
        }
    }

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private int sortingOrder = 100;
    
    [Header("Camera Settings")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float marginMultiplier = 1.2f; 
    private List<Bullet> _bulletPool = new List<Bullet>();
    
    private int _activeBulletsCount = 0;      // Total de balas activas
    private int _visibleBulletsCount = 0;     // Solo balas visibles en cámara
    
    public int ActiveBulletsCount => _activeBulletsCount;
    public int VisibleBulletsCount => _visibleBulletsCount;
   
    private Vector2 _cameraMin;
    private Vector2 _cameraMax;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
        
        AddBulletsToPool(poolSize);
    }

    private void AddBulletsToPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            _bulletPool.Add(bullet);
            bullet.transform.parent = transform;
            
            SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = sortingOrder;
            }
        }
    }

    public Bullet RequestBullet()
    {
        for (int i = 0; i < _bulletPool.Count; i++)
        {
            if (!_bulletPool[i].gameObject.activeInHierarchy)
            {
                _bulletPool[i].gameObject.SetActive(true);
                return _bulletPool[i];
            }
        }
        
        AddBulletsToPool(1);
        _bulletPool[_bulletPool.Count - 1].gameObject.SetActive(true);
        return _bulletPool[_bulletPool.Count - 1];
    }
    
    private void UpdateCameraBounds()
    {
        if (targetCamera == null) return;
        
        float height = targetCamera.orthographicSize * 2f * marginMultiplier;
        float width = height * targetCamera.aspect;
        
        Vector3 cameraPos = targetCamera.transform.position;
        
        _cameraMin = new Vector2(cameraPos.x - width / 2f, cameraPos.y - height / 2f);
        _cameraMax = new Vector2(cameraPos.x + width / 2f, cameraPos.y + height / 2f);
    }
    
    private void LateUpdate()
    {
        UpdateCameraBounds();
        UpdateBulletCounts();
    }
    
    private void UpdateBulletCounts()
    {
        _activeBulletsCount = 0;
        _visibleBulletsCount = 0;
        
        foreach (Bullet bullet in _bulletPool)
        {
            if (bullet != null && bullet.gameObject.activeInHierarchy)
            {
                _activeBulletsCount++;
                
                
                Vector2 pos = bullet.transform.position;
                if (IsInsideCameraBounds(pos))
                {
                    _visibleBulletsCount++;
                }
            }
        }
    }
    
    private bool IsInsideCameraBounds(Vector2 position)
    {
        return position.x >= _cameraMin.x && position.x <= _cameraMax.x &&
               position.y >= _cameraMin.y && position.y <= _cameraMax.y;
    }
    
    public int GetTotalPoolSize()
    {
        return _bulletPool.Count;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log($"Balas TOTALES activas: {_activeBulletsCount}");
            Debug.Log($"Balas VISIBLES en cámara: {_visibleBulletsCount}");
            Debug.Log($"Pool size: {_bulletPool.Count}");
        }
    }
    
    private void OnDrawGizmos()
    {
        if (targetCamera == null) return;
        
        UpdateCameraBounds();
        
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3(
            (_cameraMin.x + _cameraMax.x) / 2f,
            (_cameraMin.y + _cameraMax.y) / 2f,
            0f
        );
        Vector3 size = new Vector3(
            _cameraMax.x - _cameraMin.x,
            _cameraMax.y - _cameraMin.y,
            0.1f
        );
        Gizmos.DrawWireCube(center, size);
    }
}