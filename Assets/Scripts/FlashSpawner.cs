using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlashElement;
using UnityEngine;

public class FlashSpawner : ObjectPool<Flash>
{
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _sequenceRepeatDelay;
    [SerializeField] private Flash _prefab;
    [SerializeField] private SpawnArea _spawnArea;
    [SerializeField] private int _capacity;

    private List<Flash> _spawnedObjects = new();
    private IEnumerator _spawnCoroutine;
    private IEnumerator _sequenceSpawnCoroutine;
    private List<Flash> _sequenceObjects = new();

    public event Action FullSequenceShowed;

    public List<Flash> SequenceObjects => _sequenceObjects;

    private void Awake()
    {
        for (int i = 0; i < _capacity; i++)
        {
            Initalize(_prefab);
        }
    }

    public void StartSpawnMultiple()
    {
        StopSpawnMultiple();

        _spawnCoroutine = SpawningMultipleCoroutine();
        StartCoroutine(_spawnCoroutine);
    }

    public void StopSpawnMultiple()
    {
        if (_spawnCoroutine == null)
            return;

        StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = null;
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

    public void StartSequenceSpawn(int count)
    {
        StopSequenceSpawn();

        _sequenceSpawnCoroutine = SequenceSpawningCoroutine(count);
        StartCoroutine(_sequenceSpawnCoroutine);
    }

    public void StopSequenceSpawn()
    {
        if (_sequenceSpawnCoroutine == null)
            return;

        StopCoroutine(_sequenceSpawnCoroutine);
        _sequenceSpawnCoroutine = null;

        foreach (var item in _sequenceObjects)
        {
            ReturnToPool(item);
        }

        _sequenceObjects.Clear();
    }


    private IEnumerator SpawningCoroutine()
    {
        WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            Spawn(_spawnedObjects);
            yield return interval;
        }
    }
    
    private IEnumerator SpawningMultipleCoroutine()
    {
        WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            Spawn(_spawnedObjects);
            Spawn(_spawnedObjects);
            yield return interval;
        }
    }

    private void Spawn(List<Flash> listToAdd)
    {
        if (ActiveObjects.Count >= _capacity)
            return;

        if (TryGetObject(out Flash flash, _prefab))
        {
            flash.SetScaleAndType();
            flash.transform.position = _spawnArea.GetPositionToSpawn();
            flash.Disabled += ReturnToPool;
            listToAdd.Add(flash);
        }
    }

    private IEnumerator SequenceSpawningCoroutine(int count)
    {
        if (_sequenceObjects.Count == 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (TryGetObject(out Flash flash, _prefab))
                {
                    flash.SetScaleAndType();
                    flash.transform.position = _spawnArea.GetMiddlePositionToSpawn();
                    flash.Disabled += ReturnToPool;
                    _sequenceObjects.Add(flash);
                    // Add a delay after spawning each Flash
                    yield return new WaitForSeconds(2);
                }
            }
        }
        
        //Debug.Log(_sequenceObjects.Count);
        
        if (_sequenceObjects.Count == count)
            FullSequenceShowed?.Invoke();

        WaitForSeconds repeatDelay = new WaitForSeconds(_sequenceRepeatDelay);

        while (true)
        {
            List<Flash> sequenceCopy = new List<Flash>(_sequenceObjects);

            foreach (Flash flash in sequenceCopy)
            {
                if (flash != null)
                {
                    flash.gameObject.SetActive(true);
                    flash.transform.position = _spawnArea.GetMiddlePositionToSpawn();

                    // Add delay between flashes during the sequence playback
                    yield return new WaitForSeconds(_spawnInterval);

                    // Deactivate the flash after displaying
                    flash.gameObject.SetActive(false);
                }
            }

            // Add delay before repeating the sequence
            yield return repeatDelay;
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

        if (_sequenceObjects.Count > 0)
        {
            foreach (Flash flash in _sequenceObjects)
            {
                ReturnToPool(flash);
            }
        }
    }

    public void DecreaseSpawnInterval()
    {
        _spawnInterval = Mathf.Max(0.1f, _spawnInterval - 0.3f);
    }
}