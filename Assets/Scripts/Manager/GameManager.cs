using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int _stage;
    private int _level;
    private int _score;
    private bool _isGameOver;
    private bool _isGamePause;
    private float saveTimeScale;
    public Text scoreText;
    public Text levelText;
    public BossSpawner bossSpawner;
    
    private AudioSource _audio;

    public GameObject gameOverPanel;
    public GameObject joyStick;
    public Action OnStageChange;
    public Action OnBossSpawn;
    
    public int Stage
    {
        get => _stage;
        set => _stage = value;
    }
    
    public int Score
    {
        get => _score;
        set => _score = value;
    }
    
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
        Application.targetFrameRate = 60;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStart();
        var player = FindFirstObjectByType<PlayerStatManager>();
        player.UpdateHealthUI();
        OnBossSpawn += bossSpawner.SpawnBoss;
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
        

        if (_level == 21)
        {
            var player = FindFirstObjectByType<PlayerAttackManager>();
            player.SetAttackStrategy(new ThreeWayAttack());
            OnBossSpawn?.Invoke();
        }
        else if (_level == 41)
        {
            var player = FindFirstObjectByType<PlayerAttackManager>();
            player.SetAttackStrategy(new FourWayAttack());
            OnBossSpawn?.Invoke();
        }
        else if (_level == 61)
        {
            var player = FindFirstObjectByType<PlayerAttackManager>();
            player.SetAttackStrategy(new SevenWayAttack());
            OnBossSpawn?.Invoke();
        }
        else if (_level == 81)
        {
            var player = FindFirstObjectByType<PlayerAttackManager>();
            player.SetAttackStrategy(new FinalAttack());
            OnBossSpawn?.Invoke();
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
        _isGameOver = true;
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.Find("Record").GetComponent<Text>().text = _score.ToString();
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
    }

    public void GameRestart()
    {
        LoadingController.LoadScene("Scenes/GameScene");
    }
    
    public void GameExit()
    {
        SceneManager.LoadScene("Scenes/LobbyScene");
    }
}
