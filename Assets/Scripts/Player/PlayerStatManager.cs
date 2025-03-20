using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour, IStatable, ILevel
{
    [Header("Stat")]
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int attack = 20;
    [SerializeField] private float attackRate = .3f;
    [SerializeField] private float speed = 5f;
    
    [Header("Level")]
    [SerializeField] private int level = 1;
    [SerializeField] private int experience = 0;
    [SerializeField] private int experienceToNextLevel = 10;
    
    private EPlayerState _pawnState;
    private SpriteRenderer _playerSprite;
    
    public Slider healthSlider;
    public Slider experienceSlider;

    public Sprite upGrade1;
    public Sprite upGrade2;

    public Action OnLevelUp;
    public Action OnDeath;
    
    public int Health
    {
        get => health;
        set => health = value;
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

    private void Awake()
    {
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    
    public void Damage(int amount)
    {
        if(_pawnState is (EPlayerState.Dead or EPlayerState.Damaged))
        {
            return;
        }
        
        health -= amount;
        
        healthSlider.value = (float)health / maxHealth;

        if (health <= 0)
        {
            Die();
        }
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
        health = maxHealth;
        attack += 10;
        attackRate -= 0.001f;
        healthSlider.value = (float)health / maxHealth;
        OnLevelUp?.Invoke();
        Time.timeScale += 0.01f;

        if (level == 20)
        {
            _playerSprite.sprite = upGrade1;
        }
        else if (level == 40)
        {
            _playerSprite.sprite = upGrade2;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            Damage(enemy.enemyData.Damage);
        }
    }
}
