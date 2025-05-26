using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BossSpawner : MonoBehaviour
{
    public GameObject[] bossPrefabs;
    public int bossIndex = 0;
    public float spawnInterval = 5f; // 보스 소환 간격 (초)
    public GameObject finalBossPrefab;
    private float _time;
    private int _stage = 0;

    void Start()
    {
        GameManager.Instance.OnSpawnerUpgrade += () => _stage++;
        GameManager.Instance.OnFinalStage += FinalBoss;
    }

    private void Update()
    {
        if (_stage == 0) return;

        _time += Time.deltaTime;
        if (_time >= spawnInterval)
        {
            Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length - 2)], transform.position, Quaternion.identity);
            _time = 0;
        }
    }

    public void SpawnBoss()
    {
        if (bossIndex < 0 || bossIndex >= bossPrefabs.Length)
        {
            return;
        }
        
        Instantiate(bossPrefabs[bossIndex++], transform.position, Quaternion.identity);
    }

    public void FinalBoss()
    {
        Instantiate(finalBossPrefab, transform.position, Quaternion.identity);
    }
}
