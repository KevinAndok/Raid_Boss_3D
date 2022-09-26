using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingSystem : MonoBehaviour
{
    public List<PooledPrefab> pooledObjects = new List<PooledPrefab>();

    public static List<Pool> Pools = new List<Pool>();

    private void Awake()
    {
        Pools.Clear();

        foreach (PooledPrefab prefab in pooledObjects)
        {
            Pools.Add(new Pool { poolName = prefab.name, objectPrefab = prefab.prefab });
        }
    }
    private void OnDisable() => ClearAllPools();

    public static Pool GetPoolByName(string name)
    {
        foreach (Pool p in Pools) if (p.poolName == name) return p;

        Debug.LogWarning("Pool not found");
        return null;
    }
    public void ClearAllPools()
    {
        foreach (Pool pool in Pools) pool.ObjectPool.Clear();
    }
    public void ClearPoolByName(string poolName)
    {
        foreach (Pool p in Pools)
        {
            if (p.poolName == name)
            {
                p.ObjectPool.Clear();
                return;
            }
        }

        Debug.LogWarning("Pool not found");
    }
    public void ClearPool(Pool pool)
    {
        foreach (Pool p in Pools)
        {
            if (p == pool)
            {
                p.ObjectPool.Clear();
                return;
            }
        }

        Debug.LogWarning("Pool not found");
    }
}

[Serializable]
public class PooledPrefab
{
    public string name;
    public GameObject prefab;
}

public class Pool
{
    public string poolName;
    public GameObject objectPrefab;
    IObjectPool<GameObject> _objectPool;
    public IObjectPool<GameObject> ObjectPool
    {
        get
        {
            if (_objectPool == null)
                _objectPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool);
            return _objectPool;
        }
    }

    GameObject CreatePooledItem() => GameObject.Instantiate(objectPrefab);
    void OnTakeFromPool(GameObject _gameObject) => _gameObject.SetActive(true);
    void OnReturnedToPool(GameObject _gameObject) => _gameObject.SetActive(false);
}
