using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonBase<PoolManager>
{
    private Dictionary<int, Stack<GameObject>> _pools = new();
    private Dictionary<int, int> _objToKey = new();
    public GameObject poolParent;
    public override void PostAwake() { }
    public GameObject GetObject(GameObject prefab)
    {
         Debug.Log($" {prefab.name} @ Get Object");
        // Make sure a stack exists for this prefab
        int key = Animator.StringToHash(prefab.name);
        if (!_pools.ContainsKey(key)) {
            _pools[key] = new Stack<GameObject>();
            Debug.Log("Initalzing new stack @ Get Object");
        }

        // Get the stack for this prefab
        Stack<GameObject> pool = _pools[key];

        // Either pop from stack or create new instance
        // GameObject obj = (pool.Count > 0) ? pool.Pop() : Instantiate(prefab, poolParent.transform);
        GameObject obj; 
        if (pool.Count > 0) {
            obj = pool.Pop();
            Debug.Log($"Returning a Object @ Get Object: Stack size {pool.Count}");
        } else {
            obj = Instantiate(prefab, poolParent.transform);
            Debug.Log("Making a new Object @ Get Object");
        }
        // Activate the object
        obj.SetActive(true);
        int SecKey = Animator.StringToHash(obj.name);
        Debug.Log($"New key : {SecKey} to {key}");
        _objToKey[SecKey] = key;

        return obj;
    }

    public GameObject GetObject(GameObject prefab, Vector3 pos, Quaternion quaternion)
    {
        GameObject retval = GetObject(prefab);
        retval.transform.SetPositionAndRotation(pos, quaternion);
        return retval;
    }

    // Return object to the pool
    public void ReturnObject(GameObject obj)
    {

        int Seckey = Animator.StringToHash(obj.name);
        int key = _objToKey[Seckey];
        Debug.Log($"{obj} @ Key {key} @ Return Object");
        
        // Ensure we have a stack for this prefab
        if (!_pools.ContainsKey(key)) {
            _pools[key] = new Stack<GameObject>();
            Debug.Log("Initalzing new stack @ Return Object");
        }
        else Debug.Log("Base in pool @ Return Object");

        // Deactivate the object
        obj.SetActive(false);

        // Add to the stack
        _pools[key].Push(obj);
    }

    // Pre-populate the pool with a number of instances
    public void PreWarm(GameObject prefab, int count)
    {
        int key = Animator.StringToHash(prefab.name);
        if (!_pools.ContainsKey(key)) _pools[key] = new Stack<GameObject>();


        Stack<GameObject> pool = _pools[key];

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, poolParent.transform);
            obj.SetActive(false);
            pool.Push(obj);
        }
    }
}