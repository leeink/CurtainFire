using System;
using UnityEngine;
using UnityEngine.Pool;

public class BossAttackManager : MonoBehaviour
{
    [SerializeField]private EnemyBullet bulletPrefab;
    [SerializeField]private int defaultPoolSize;
    [SerializeField]private int maxPoolSize;
    private IObjectPool<EnemyBullet> _pool;
    public Transform enemyTransform;

    public IObjectPool<EnemyBullet> Pool => _pool;

    private void Awake()
    {
        _pool = new ObjectPool<EnemyBullet>(
            CreateBullet, OnGetBullet,
            OnReleaseBullet, OnDestroyBullet,
            true, defaultPoolSize, maxPoolSize
        );
    }

    EnemyBullet CreateBullet()
    {
        EnemyBullet bullet = Instantiate(bulletPrefab, enemyTransform.position, Quaternion.identity);
        bullet.SetManagedPool(_pool);
        return bullet;
    }

    void OnGetBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    
    void OnReleaseBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    
    void OnDestroyBullet(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
