using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectPool : MonoBehaviour
{
    // initial number of cloned objects
    [SerializeField] private uint initPoolSize = 10;
    public uint InitPoolSize => initPoolSize;

    // PooledObject prefab
    [SerializeField] private ProjectilePooledObject objectToPool;

    // store the pooled objects in stack
    private Stack<ProjectilePooledObject> stack;

    private void Start()
    {
        SetupPool();
    }

    void SetupPool()
    {
        // missing objectToPool Prefab field
        if (objectToPool == null)
        {
            return;
        }

        stack = new Stack<ProjectilePooledObject>();

        // populate the pool
        ProjectilePooledObject instance = null;

        for (int i = 0; i < InitPoolSize; i++)
        {
            instance = Instantiate(objectToPool);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }

    // returns the first GameObject from the pool
    public ProjectilePooledObject GetPooledObject()
    {
        // missing objectToPool Prefab field
        if (objectToPool == null)
        {
            return null;
        }

        ProjectilePooledObject instance = null;
        if (stack.Count <= 0)
        {
            // if the pool is not large enough, instantiate extra PooledObjects
            instance = Instantiate(objectToPool);
            instance.Pool = this;
        }
        else
        {
            // otherwise, return the next instance from the stack
            instance = stack.Pop();
        }

        instance.gameObject.SetActive(true);
        return instance;
    }

    public void ReturnToPool(ProjectilePooledObject pooledObject)
    {
        // Deactivate
        pooledObject.gameObject.SetActive(false);
        // Push back to the stack
        stack.Push(pooledObject);
    }

}
