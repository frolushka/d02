using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Townhall : MonoBehaviour
{
    [SerializeField] private float spawnDelay;
    [SerializeField] private float spawnTimeIncreasing;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform spawnPosition;

    private float _currentSpawnDelay;
    private float _nextSpawnDelay;
    
    private void Start()
    {
        _currentSpawnDelay = spawnDelay;
        _nextSpawnDelay = _currentSpawnDelay;
    }

    private void Update()
    {
        _nextSpawnDelay -= Time.deltaTime;
        if (_nextSpawnDelay <= 0)
        {
            SpawnUnit();
            _nextSpawnDelay = _currentSpawnDelay;
        }
    }

    private void SpawnUnit()
    {
        Instantiate(unitPrefab, spawnPosition.position, Quaternion.identity);
    }

    public void IncreaseSpawnTime()
    {
        _currentSpawnDelay += spawnTimeIncreasing;
    }

    public void Die()
    {
        Debug.Log($"The {(((1 << gameObject.layer) & LayerMask.GetMask("HumanBuilding")) != 0 ? "Orc" : "Human")} Team wins.");
    }
}
