using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages object pooling for GameObjects.
/// </summary>
public class PoolManager : SingletonBase<PoolManager>
{
    //  ------------------ Private ------------------
    private Dictionary<int, Stack<GameObject>> _pools = new();
    private Dictionary<int, int> _objToKey = new();

    //  ------------------ Public ------------------
    [Header("Pool Settings")]
    [Tooltip("Parent GameObject where all pooled objects will be stored.")]
    public GameObject poolParent;

    /// <summary>
    /// Called after the singleton instance is initialized.
    /// </summary>
    public override void PostAwake() { }

    /// <summary>
    /// Retrieves an object from the pool or instantiates a new one if none are available.
    /// </summary>
    /// <param name="prefab">The prefab to pool.</param>
    /// <param name="parent">The parent transform for the instantiated object.</param>
    /// <returns>An active GameObject instance.</returns>
    public GameObject GetObject(GameObject prefab, Transform parent = null)
    {
        // Create a key for the prefab using its name hash.
        int key = Animator.StringToHash(prefab.name);

        // Ensure a stack exists for this prefab.
        if (!_pools.ContainsKey(key)) _pools[key] = new Stack<GameObject>();

        // Retrieve the pool for this prefab.
        Stack<GameObject> pool = _pools[key];

        // Retrieve an object from the pool or instantiate a new one.
        GameObject obj = pool.Count > 0 ? pool.Pop() : Instantiate(prefab);

        // Set parent correctly while retaining local transformation.
        obj.transform.SetParent(parent != null ? parent : poolParent.transform, false);
        // Activate the object.
        obj.SetActive(true);

        // Map the object to its pool key.
        int secKey = Animator.StringToHash(obj.name);
        _objToKey[secKey] = key;

        return obj;
    }

    /// <summary>
    /// Retrieves an object from the pool or instantiates a new one, then sets its position and rotation.
    /// </summary>
    /// <param name="prefab">The prefab to pool.</param>
    /// <param name="pos">The position to set.</param>
    /// <param name="quaternion">The rotation to set.</param>
    /// <param name="parent">Optional parent transform.</param>
    /// <returns>An active GameObject instance with updated transform.</returns>
    public GameObject GetObject(GameObject prefab, Vector3 pos, Quaternion quaternion, Transform parent = null)
    {
        GameObject retval = GetObject(prefab, parent);
        retval.transform.SetPositionAndRotation(pos, quaternion);
        return retval;
    }

    /// <summary>
    /// Returns an object back to its pool.
    /// </summary>
    /// <param name="obj">The object to return.</param>
    public void ReturnObject(GameObject obj)
    {
        int secKey = Animator.StringToHash(obj.name);
        int key = _objToKey[secKey];

        // Ensure the pool exists for this prefab.
        if (!_pools.ContainsKey(key)) _pools[key] = new Stack<GameObject>();
        // Reset parent to the pool parent while preserving local transformation.
        obj.transform.SetParent(poolParent.transform, false);
        // Deactivate the object.
        obj.SetActive(false);

        // Add the object back into the pool.
        _pools[key].Push(obj);
    }

    /// <summary>
    /// Pre-populates the pool with a specified number of instances of the prefab.
    /// </summary>
    /// <param name="prefab">The prefab to pre-warm.</param>
    /// <param name="count">The number of instances to create.</param>
    public void PreWarm(GameObject prefab, int count)
    {
        int key = Animator.StringToHash(prefab.name);
        if (!_pools.ContainsKey(key))
            _pools[key] = new Stack<GameObject>();

        Stack<GameObject> pool = _pools[key];

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, poolParent.transform);
            obj.SetActive(false);
            pool.Push(obj);
        }
    }
}
