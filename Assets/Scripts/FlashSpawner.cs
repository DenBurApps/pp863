using System.Collections;
using System.Collections.Generic;
using FlashElement;
using UnityEngine;

public class FlashSpawner : ObjectPool<Flash>
{
    [SerializeField] private float _spawnInterval;
    [SerializeField] private Flash _prefab;
    [SerializeField] private SpawnArea _spawnArea;
    [SerializeField] private int _capacity;

    private List<Flash> _spawnedObjects = new();

    private IEnumerator _spawnCoroutine;

    private void Awake()
    {
        for (int i = 0; i < _capacity; i++)
        {
            Initalize(_prefab);
        }
    }

    public void StartSpawn()
    {
        StopSpawn();

        _spawnCoroutine = SpawningCoroutine();
        StartCoroutine(_spawnCoroutine);
    }

    public void StopSpawn()
    {
        if (_spawnCoroutine == null)
            return;

        StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = null;
    }

    private IEnumerator SpawningCoroutine()
    {
        WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            Spawn();
            yield return interval;
        }
    }

    private void Spawn()
    {
        if (ActiveObjects.Count >= _capacity)
            return;

        if (TryGetObject(out Flash flash, _prefab))
        {
            flash.transform.position = _spawnArea.GetPositionToSpawn();
            flash.Disabled += ReturnToPool;
            _spawnedObjects.Add(flash);
        }
    }
        
    public void ReturnToPool(Flash flash)
    {
        if (flash == null)
            return;
            
        flash.Disabled -= ReturnToPool;
        PutObject(flash);

        if (_spawnedObjects.Contains(flash))
            _spawnedObjects.Remove(flash);
    }

    public void ReturnAllObjectsToPool()
    {
        if (_spawnedObjects.Count <= 0)
            return;

        List<Flash> objectsToReturn = new List<Flash>(_spawnedObjects);
        foreach (var @object in objectsToReturn)
        {
            ReturnToPool(@object);
        }
    }

    public void DecreaseSpawnInterval()
    {
        _spawnInterval -= 0.3f;
    }
}