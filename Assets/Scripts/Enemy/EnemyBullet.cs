using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour
{
    public BossBullet bulletData;
    private IObjectPool<EnemyBullet> _managedPool;
    private int _extraDamage;
    private Vector3 _direction;
    private bool isReleased = false;
    
    public Vector3 Direction
    {
        get => _direction;
        set => _direction = value;
    }
    
    private void Awake()
    {
    }
    
    void OnEnable()
    {
        transform.position = GameObject.FindAnyObjectByType<Step1Boss>().transform.position;
        DisableBullet();
    }

    private void OnDisable()
    {
        isReleased = false;
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
        Invoke(nameof(DestroyBullet), 5f);
    }
    
    public void SetManagedPool(IObjectPool<EnemyBullet> pool)
    {
        _managedPool = pool;
    }
    
    public void DestroyBullet()
    {
        if (!isReleased)
        {
            isReleased = true;
            // 풀에 반환하는 코드
            _managedPool.Release(this);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other is not null && other.gameObject.CompareTag("Player"))
        {
            CancelInvoke(nameof(DestroyBullet));
            PlayerStatManager player = other.gameObject.GetComponent<PlayerStatManager>();
            player.OnDamage(bulletData.Damage + _extraDamage);
            Debug.Log("Hit Player");
            Invoke(nameof(DestroyBullet), 0.01f);
        }
    }
}
