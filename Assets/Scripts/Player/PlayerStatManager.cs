using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerStatManager : MonoBehaviour, IStatable, ILevel, ILivingEntity
{
    [Header("Stat")] private int _health;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int attack = 20;
    [SerializeField] private float attackRate = .3f;
    [SerializeField] private float speed = 5f;
    
    [Header("Level")]
    [SerializeField] private int level = 1;
    [SerializeField] private int experience = 0;
    [SerializeField] private int experienceToNextLevel = 10;
    
    [Header("SFX")]
    private AudioSource _audioSource;
    public AudioClip spaceUpgradeClip;
    
    [Header("LevelUpAsset")]
    public Sprite upGrade1;
    public Sprite upGrade2;
    public GameObject levelUpEffect;
    private SpriteRenderer _playerSprite;
    
    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public Slider experienceSlider;
    
    private EPlayerState _pawnState;
    
    public Action OnLevelUp;
    public Action OnDeath;
    
    public int Health
    {
        get => _health;
        set => _health = value;
    }
    
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
    
    public int Attack
    {
        get => attack;
        set => attack = value;
    }
    
    public float AttackRate
    {
        get => attackRate;
        set => attackRate = Mathf.Clamp(value, 0.2f, 0.25f);
    }
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 5f, 20f);
    }
    
    public int Level
    {
        get => level;
        set => level = value;
    }

    public EPlayerState PawnState
    {
        get => _pawnState;
        set => _pawnState = value;
    }

    private void Awake()
    {
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Health = maxHealth;
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        _health += amount;
        if (_health > maxHealth)
        {
            _health = maxHealth;
        }

        UpdateHealthUI();
    }

    public void OnDamage(int amount)
    {
        if(_pawnState is (EPlayerState.Dead or EPlayerState.Damaged))
        {
            return;
        }
        
        _health -= amount;
        _pawnState = EPlayerState.Damaged;
        StartCoroutine(SetIdle());

        UpdateHealthUI();
        
        if (_health <= 0)
        {
            Die();
        }
    }
    
    private IEnumerator SetIdle()
    {
        _pawnState = EPlayerState.Idle;
        yield return new WaitForSeconds(1f);
    }

    public void Die()
    {
        _pawnState = EPlayerState.Dead;
        OnDeath?.Invoke();
    }
    
    public void SpeedUp(int amount)
    {
        speed += amount;
    }
    
    public void SpeedDown(int amount)
    {
        speed -= amount;
    }
    
    public void GainExperience(int amount)
    {
        experience += amount;
        
        experienceSlider.value = (float)experience / experienceToNextLevel;
        
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    
    public void LevelUp()
    {
        level++;
        experience = 0;
        experienceToNextLevel = (int) (experienceToNextLevel * 1.2f);
        maxHealth += 50;
        attack += 25;
        attackRate -= 0.0005f;
        _health = maxHealth;
        healthSlider.value = (float)_health / maxHealth;
        OnLevelUp?.Invoke();
        Time.timeScale += 0.01f;

        if (level == 20)
        {
            StartCoroutine(LevelUpSpace(upGrade1));
        }
        else if (level == 40)
        {
            StartCoroutine(LevelUpSpace(upGrade2));
        }
        
        UpdateHealthUI();
    }

    private IEnumerator LevelUpSpace(Sprite sprite)
    {
        levelUpEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _playerSprite.sprite = sprite;
        levelUpEffect.SetActive(false);
        _audioSource.PlayOneShot(spaceUpgradeClip);
    }

    public void UpdateHealthUI()
    {
        healthSlider.value = (float)_health / maxHealth;
        healthText.text = $"{_health}  /  {maxHealth}";
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            OnDamage(enemy.enemyData.Damage);
            enemy.DestroyEnemy();
        }
    }
}
