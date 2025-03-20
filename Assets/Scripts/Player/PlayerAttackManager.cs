using UnityEngine;
using UnityEngine.Pool;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField]private Bullet bulletPrefab;
    [SerializeField]private int defaultPoolSize;
    [SerializeField]private int maxPoolSize;
    private IObjectPool<Bullet> _pool;
    public Transform playerTransform;
    
    private EPlayerState _playerState;
    private float _time;
    
    private PlayerStatManager _playerStatManager;
    private AttackContext _attackContext;
    private AudioSource _audio;
    public AudioClip attackSound;
    private ThreeWayAttack _threeWayAttack;

    private void Awake()
    {
        _attackContext = GetComponent<AttackContext>();
        _pool = new ObjectPool<Bullet>(
            CreateBullet, OnGetBullet,
            OnReleaseBullet, OnDestroyBullet,
            true, defaultPoolSize, maxPoolSize
            );
    }

    private void Start()
    {
        _time = 0;
        _playerStatManager = GetComponent<PlayerStatManager>();
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Attack();
    }

    Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, playerTransform.position, Quaternion.identity);
        bullet.SetManagedPool(_pool);
        return bullet;
    }

    void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    
    void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    
    void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void Attack()
    {
        if (_playerState is EPlayerState.Dead or EPlayerState.Reload)
            return;
        
        _time += Time.deltaTime;
        if (_time >= _playerStatManager.AttackRate)
        {
            _attackContext.AttackStrategy(_pool, playerTransform);
            _audio.PlayOneShot(attackSound);
            _time = 0;
        }
    }
    
    public void SetAttackStrategy(IAttackable attackStrategy)
    {
        _attackContext.setAttackStrategy(attackStrategy);
    }
}
