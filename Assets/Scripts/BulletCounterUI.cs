using UnityEngine;
using TMPro; 

public class BulletCounterUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI bulletCountText; 
    
    [Header("Display Settings")]
    [SerializeField] private string prefix = "Balas: ";
    [SerializeField] private bool showOnlyVisible = true; // ← NUEVO: mostrar solo visibles
    [SerializeField] private bool showPoolInfo = true;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;
    [SerializeField] private int warningThreshold = 100;
    [SerializeField] private int dangerThreshold = 200;
    
    private void Update()
    {
        if (BulletPool.Instance == null)
        {
            if (bulletCountText != null)
                bulletCountText.text = "Pool no encontrado";
            return;
        }
        
        UpdateBulletCount();
    }
    
    private void UpdateBulletCount()
    {
        int bulletCount = showOnlyVisible 
            ? BulletPool.Instance.VisibleBulletsCount 
            : BulletPool.Instance.ActiveBulletsCount;
        
        if (bulletCountText != null)
        {
            if (showPoolInfo)
            {
                int totalPool = BulletPool.Instance.GetTotalPoolSize();
                string displayType = showOnlyVisible ? "Visibles" : "Activas";
                bulletCountText.text = $"{prefix}{bulletCount} {displayType}";
            }
            else
            {
                bulletCountText.text = $"{prefix}{bulletCount}";
            }
            
            UpdateTextColor(bulletCount);
        }
    }
    
    private void UpdateTextColor(int bulletCount)
    {
        if (bulletCount >= dangerThreshold)
        {
            bulletCountText.color = dangerColor;
        }
        else if (bulletCount >= warningThreshold)
        {
            bulletCountText.color = warningColor;
        }
        else
        {
            bulletCountText.color = normalColor;
        }
    }
    
    public void SetPrefix(string newPrefix)
    {
        prefix = newPrefix;
    }
}