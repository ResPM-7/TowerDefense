using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolManager<T> : MonoBehaviour where T : Component
{
    public T prefab;

    public int initialSize = 10;

    private Queue<T> pool = new Queue<T>();

    private void Start()
    {
        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            pool.Enqueue(obj);
        }
    }


    private T CreateNewObject()
    {
        T obj = Instantiate(prefab, transform);
        obj.gameObject.SetActive(false);

        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.Init(() => ReturnToPool(obj));
        }

        return obj;
    }

    public T GetObject(Vector3 position, Quaternion rotation)
    {
        T obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = CreateNewObject();
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);

        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnSpawn();
        }
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);

        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.OnDespawn();
        }

        pool.Enqueue(obj);
    }
}



public interface IPoolable
{
    void Init(Action returnAction);
    void OnSpawn();
    void OnDespawn();
}