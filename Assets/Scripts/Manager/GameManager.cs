using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int _stage;
    private int _level;
    private int _score;
    private bool _isGameOver;
    private bool _isGamePause;
    private float saveTimeScale;
    
    public BossSpawner bossSpawner;
    
    [Header("UI")]
    public Text scoreText;
    public Text levelText;
    public GameObject gameOverPanel;
    public GameObject joyStick;
    
    [Header("Event Sounds")]
    private AudioSource _audio;
    public AudioClip gameClearClip;
    public AudioClip gameOverClip;
    public AudioClip gameResumeClip;
    
    public Action OnStageChange;
    private Action _onBossSpawn;
    public Action OnFinalStage;
    public Action OnSpawnerUpgrade;
    
    public bool IsGameOver
    {
        get => _isGameOver;
        set => _isGameOver = value;
    }
    
    public bool IsGamePause
    {
        get => _isGamePause;
        set => _isGamePause = value;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _audio = GetComponent<AudioSource>();
        _audio.ignoreListenerPause = true;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStart();
        var player = FindFirstObjectByType<PlayerStatManager>();
        player.UpdateHealthUI();
        _onBossSpawn += bossSpawner.SpawnBoss;
        player.OnDeath += GameOver;
        player.OnLevelUp += NextLevel;
    }
    
    public void AddScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();
    }

    private void NextLevel()
    {
        _level++;
        levelText.text = _level.ToString();
        if (_level % 10 == 0)
        {
            _stage++;
            OnStageChange?.Invoke();
        }
        
        if (_level == 20)
        {
            var player = FindFirstObjectByType<PlayerAttackManager>();
            player.SetAttackStrategy(new ThreeWayAttack());
            _onBossSpawn?.Invoke();
        }
        else if (_level == 40)
        {
            var player = FindFirstObjectByType<PlayerAttackManager>();
            player.SetAttackStrategy(new FourWayAttack());
            _onBossSpawn?.Invoke();
        }
        else if (_level == 60)
        {
            var player = FindFirstObjectByType<PlayerAttackManager>();
            player.SetAttackStrategy(new SevenWayAttack());
            _onBossSpawn?.Invoke();
            OnSpawnerUpgrade?.Invoke();
        }
        else if (_level == 80)
        {
            _onBossSpawn?.Invoke();
        }
        else if (_level == 100)
        {
            OnFinalStage?.Invoke();
        }
    }

    private void GameStart()
    {
        _isGamePause = false;
        _isGameOver = false;
        joyStick.SetActive(true);
        
        Time.timeScale = 1;
        scoreText.text = "0";
        levelText.text = "1";
        _score = 0;
        _level = 1;
        _stage = 1;
    }
    
    public void GameOver()
    {
        GamePause();
        _audio.Stop();
        _isGameOver = true;
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.Find("Record").GetComponent<Text>().text = _score.ToString();
        _audio.PlayOneShot(gameOverClip);
    }

    public void GameClear()
    {
        GamePause();
        _audio.Stop();
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.Find("Record").GetComponent<Text>().text = _score.ToString();
        _audio.PlayOneShot(gameClearClip);
    }

    public void GamePause()
    {
        _audio.Pause();
        joyStick.SetActive(false);
        _isGamePause = true;
        saveTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    
    public void GameResume()
    {
        _isGamePause = false;
        Time.timeScale = saveTimeScale;
        _audio.PlayOneShot(gameResumeClip);
        _audio.UnPause();
    }

    public void PlayOneShotClip(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("Scenes/GameScene");
    }
    
    public void GameExit()
    {
        SceneManager.LoadScene("Scenes/LobbyScene");
    }
}
