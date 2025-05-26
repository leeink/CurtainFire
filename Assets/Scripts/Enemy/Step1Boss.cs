using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Step1Boss : MonoBehaviour, ILivingEntity, IBoss
{
    // 보스의 상태를 정의하는 열거형
    private enum BossState
    {
        Moving,
        Attacking
    }
    
    private BossState _currentState = BossState.Moving;
    private float _changeTime;
    private float _attackTime;
    private float _dirIntervalTime = 0;
    private float _attackIntervalTime = 0;
    private float _attackDuration = 1.0f; // 공격 지속 시간 (초)
    private float _currentAttackTime = 0f; // 공격 진행 시간
    private Vector2 direction;
    private float _speed;
    private Vector2 _positionYRange = new Vector2(-4.27f, 4.27f);
    [SerializeField] private int health = 1000;
    
    private BossAttackManager _bossAttackManager;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audio;

    public AudioClip hitSound;
    public Canvas UnitCanvas;
    
    public int Health
    {
        get => health;
        set => health = value;
    }
    
    void Awake()
    {
        UnitCanvas.gameObject.SetActive(false);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bossAttackManager = GetComponent<BossAttackManager>();
    }

    void OnEnable()
    {
        _spriteRenderer.enabled = true;
    }
    
    void Start()
    {
        _changeTime = Random.Range(1f, 3f);
        _attackTime = Random.Range(2f, 4f);
        direction = Vector2.up;
        _speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        _attackIntervalTime += Time.deltaTime;
        
        // 현재 상태에 따라 다른 행동을 수행
        switch(_currentState)
        {
            case BossState.Moving:
                // 이동 중일 때는 BaseAction을 수행하고 공격 타이밍을 체크
                BaseAction();
                
                // 공격할 시간이 되면 상태를 Attacking으로 전환
                if (_attackIntervalTime >= _attackTime)
                {
                    _currentState = BossState.Attacking;
                    _currentAttackTime = 0f;
                    AttackPattern();  // 공격 패턴 실행
                    _attackIntervalTime = 0; // 공격 간격 타이머 초기화
                }
                break;
                
            case BossState.Attacking:
                // 공격 중일 때는 이동하지 않고 공격 지속 시간을 체크
                _currentAttackTime += Time.deltaTime;
                
                // 공격 지속 시간이 끝나면 다시 이동 상태로 전환
                if (_currentAttackTime >= _attackDuration)
                {
                    _currentState = BossState.Moving;
                }
                break;
        }
    }

    public void BaseAction()
    {
        _dirIntervalTime += Time.deltaTime;
        
        transform.Translate(direction * (_speed * Time.deltaTime));
        
        if (_dirIntervalTime >= _changeTime)
        {
            UpdateState(-direction.y);
        }
        if (transform.position.y >= _positionYRange.y)
        {
            transform.position = new Vector2(transform.position.x, _positionYRange.y);
            UpdateState(-1);
        }
        else if (transform.position.y <= _positionYRange.x)
        {
            transform.position = new Vector2(transform.position.x, _positionYRange.x);
            UpdateState(1);
        }
    }
    
    public void AttackPattern()
    {
        int cnt = 5;
        float angle = 60f;
        float gap = cnt > 1 ? angle / (float)(cnt - 1) : 0;
        float startAngle = -angle / 2f;

        for (int i = 0; i < cnt; ++i)
        {
            float theta = startAngle + gap * (float)i;
            theta *= Mathf.Deg2Rad;
            EnemyBullet bullet = _bossAttackManager.Pool.Get();
            Vector3 dir = new Vector3(-Mathf.Cos(theta), -Mathf.Sin(theta), 0);
            bullet.transform.rotation = Quaternion.Euler(dir * Mathf.Rad2Deg);
            bullet.Direction = dir;
        }
    }

    private void UpdateState(float value)
    {
        _dirIntervalTime = 0;
        _changeTime = Random.Range(1f, 3f);
        _speed = Random.Range(2f, 5f);
        direction = new Vector2(0, value);
    }

    public void Heal(int amount)
    {
        return;
    }

    public void OnDamage(int amount)
    {
        health -= amount;
        if (this.enabled && _audio is not null && hitSound is not null)
        {
            _audio.PlayOneShot(hitSound);
        }
        
        if (health <= 0)
        {
            _spriteRenderer.enabled = false;
            UnitCanvas.gameObject.SetActive(true);
            Invoke(nameof(Die), 0.5f);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
