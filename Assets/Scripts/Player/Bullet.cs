using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public BulletData bulletData;
    private PlayerStatManager _playerStatManager;
    private IObjectPool<Bullet> _managedPool;
    private int _extraDamage;
    private Vector3 _direction;
    
    public Vector3 Direction
    {
        get => _direction;
        set => _direction = value;
    }
    
    private void Awake()
    {
        _playerStatManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatManager>();
    }
    
    void OnEnable()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        DisableBullet();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * (Time.deltaTime * bulletData.Speed));
    }
    
    public int ExtraDamage
    {
        get => _extraDamage;
        set => _extraDamage = value;
    }

    void DisableBullet()
    {
        Invoke(nameof(DestroyBullet), 2f);
    }
    
    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        _managedPool = pool;
    }
    
    public void DestroyBullet()
    {
        _managedPool.Release(this);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other is not null && other.gameObject.CompareTag("Enemy"))
        {
            CancelInvoke(nameof(DestroyBullet));
            ILivingEntity enemy = other.gameObject.GetComponent<ILivingEntity>();
            enemy?.OnDamage(bulletData.Damage + _playerStatManager.Attack);
            Invoke(nameof(DestroyBullet), 0.001f);
        }
    }
}
