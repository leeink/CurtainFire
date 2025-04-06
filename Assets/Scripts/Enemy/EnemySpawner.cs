using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private PlayerStatManager _playerStatManager;
    
    public float initSpawnRate;
    private float _spawnRate;
    private IObjectPool<Enemy> _enemyPool;
    [SerializeField] private int defaultPoolSize;
    [SerializeField] private int maxPoolSize;
    private Vector3 _spawnPosition;
    private int _enemyIndex;
    private float _time;

    public Action OnUpgradeSpawner;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            CreateEnemy, OnGetEnemy,
            OnReleaseEnemy, OnDestroyEnemy,
            true, defaultPoolSize, maxPoolSize);
        _playerStatManager = FindFirstObjectByType<PlayerStatManager>();
    }

    void Start()
    {
        _spawnRate = initSpawnRate;
        _time = 0;
        _playerStatManager.OnLevelUp += DecreaseSpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_spawnRate <= _time)
        {
            SpawnEnemy();
            _time = 0;
        }
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab, _spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetMangedPool(_enemyPool);
        return enemy;
    }
    
    private void OnGetEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }
    
    private void OnReleaseEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemy()
    {
        _enemyPool.Get();
    }

    private void DecreaseSpawnRate()
    {
        if (_spawnRate < 0.25f) return;
        _spawnRate -= 0.002f;
    }
}
