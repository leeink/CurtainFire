using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float initSpawnRate;
    private float _spawnRate;
    private IObjectPool<Enemy> _enemyPool;
    [SerializeField] private int defaultPoolSize;
    [SerializeField] private int maxPoolSize;
    private Vector3 _spawnPosition;
    private int _enemyIndex;
    private float _time;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            CreateEnemy, OnGetEnemy,
            OnReleaseEnemy, OnDestroyEnemy,
            true, defaultPoolSize, maxPoolSize);
    }

    void Start()
    {
        _spawnRate = initSpawnRate;
        _time = 0;
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

    void SpawnEnemy()
    {
        _enemyPool.Get();
    }
}
