using UnityEngine;

public class WeaponFollowBoss : MonoBehaviour
{
    [SerializeField] private Transform boss;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float followSpeed = 10f;

    private void Start()
    {
        if (boss == null)
        {

            GameObject bossObj = GameObject.Find("Boss"); 
            if (bossObj != null)
                boss = bossObj.transform;
        }
    }

    private void LateUpdate()
    {
        if (boss == null) return;

        // Posición exacta del jefe + offset
        transform.position = boss.position + offset;
    }
}