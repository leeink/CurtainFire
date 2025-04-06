using System;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject[] bossPrefabs;
    public int bossIndex = 0;
    void Start()
    {
        
    }
    
    public void SpawnBoss()
    {
        if (bossIndex < 0 || bossIndex >= bossPrefabs.Length)
        {
            Debug.LogError("Invalid boss index");
            return;
        }
        
        GameObject boss = Instantiate(bossPrefabs[bossIndex++], transform.position, Quaternion.identity);
    }
}
