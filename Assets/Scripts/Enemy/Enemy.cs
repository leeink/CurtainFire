using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, ILivingEntity
{
    private Vector2 _direction = Vector2.left;
    private IObjectPool<Enemy> _managedPool;
    
    [Header("Prefab")]
    public EnemyData enemyData;
    [SerializeField] private Vector3[] spawnPositions;
    
    [Header("SFX")]
    private AudioSource _audio;
    public AudioClip hitSound;
    public AudioClip destroySound;
    
    private PlayerStatManager player;
    
    [Header("UI and player")]
    private SpriteRenderer _spriteRenderer;
    public Canvas UnitCanvas;
    
    private bool _isReleased;
    private int initHealth;
    private int initMaxHealth;
    private int initDamage;
    private float initSpeed;
    private int initExperience;
    private int initPoint;

    private int _health;
    private int _maxHealth;
    private int _damage;
    private float _speed;
    private int _experience;
    private int _point;

    private int _level = 1;

    public int Health
    {
        get => _health;
        set => _health = value;
    }
    
    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }
    
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    
    public int Experience
    {
        get => _experience;
        set => _experience = value;
    }
    
    public int Point
    {
        get => _point;
        set => _point = value;
    }

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindFirstObjectByType<PlayerStatManager>();
        player.OnLevelUp += StatUp;
        
        initMaxHealth = enemyData.Health;
        _health = initMaxHealth;
        initDamage = enemyData.Damage;
        initSpeed = enemyData.Speed;
        initExperience = enemyData.Experience;
        initPoint = enemyData.Point;
    }
    private void OnEnable()
    {
        UnitCanvas.gameObject.SetActive(false);
        _spriteRenderer.enabled = true;
        _isReleased = false;
        _maxHealth = initMaxHealth;
        _health = _maxHealth;
        _damage = initDamage;
        _speed = initSpeed;
        _experience = initExperience;
        _point = initPoint;
        
        int randIdx = Random.Range(0, spawnPositions.Length);
        transform.position = spawnPositions[randIdx];
        if (randIdx < spawnPositions.Length / 2)
        {
            _direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            _direction = Vector2.left;
        }
        
        Invoke(nameof(RemoveEnemy), 4f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * (enemyData.Speed * Time.deltaTime));
    }
    
    private void OnDisable()
    {
        CancelInvoke(nameof(RemoveEnemy));
    }
    
    public void SetMangedPool(IObjectPool<Enemy> pool)
    {
        _managedPool = pool;
    }

    public void RemoveEnemy()
    {
        if (!_isReleased)
        {
            _isReleased = true;
            _managedPool.Release(this);
        }
    }

    public void DestroyEnemy()
    {
        if (!_isReleased)
        {
            CancelInvoke(nameof(RemoveEnemy));
            player.GainExperience(_experience);
            GameManager.Instance.AddScore(_point);
            StartCoroutine(PlayEffectAndRemove());
            _audio.PlayOneShot(destroySound, 0.7f);
        }
    }

    private void StatUp()
    { 
        _level++;
       initMaxHealth += 10 * _level;
       initDamage += 70;
       initSpeed += 0.4f;
       initExperience = (int)((float)initExperience * 1.2);
       initPoint += 10;
       initDamage += 20;
    }

    public void Heal(int amount)
    {
        return;
    }

    public void OnDamage(int amount)
    {
        _health -= amount;
        if (this.enabled && _audio is not null && hitSound is not null)
        {
            _audio.PlayOneShot(hitSound);
        }
            
        if (_health <= 0)
        {
            DestroyEnemy();
        }
    }

    private IEnumerator PlayEffectAndRemove()
    {
        _spriteRenderer.enabled = false;
        UnitCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        RemoveEnemy();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }
}
