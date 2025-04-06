using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour
{
    public BossBullet bulletData;
    private IObjectPool<EnemyBullet> _managedPool;
    private Vector3 _direction;
    private BoxCollider2D _collider;
    private bool isReleased = false;
    
    public Vector3 Direction
    {
        get => _direction;
        set => _direction = value;
    }
    
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    
    void OnEnable()
    {
        _collider.enabled = true;
        transform.position = GameObject.FindGameObjectWithTag("Boss").transform.position;
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
        if (other.CompareTag("Boss")) return;
        
        PlayerStatManager player = other.gameObject.GetComponent<PlayerStatManager>();
        if (player is not null  && player.PawnState != EPlayerState.Damaged)
        {
            _collider.enabled = false;
            CancelInvoke(nameof(DestroyBullet));
            player.OnDamage(bulletData.Damage);
            Debug.Log("Hit Player: " + bulletData.Damage);
            Invoke(nameof(DestroyBullet), 0.001f);
        }
    }
}
