using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemBase[] itemPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    
    private readonly float _minSpawnRate = 10f;
    private float _time = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _minSpawnRate)
        {
            SpawnItem();
            _time = 0;
        }
    }

    private void SpawnItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Length);
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(itemPrefabs[randomIndex], spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);
    }
}
