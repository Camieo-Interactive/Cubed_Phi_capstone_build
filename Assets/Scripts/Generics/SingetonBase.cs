using UnityEngine;

/// <summary>
/// Base class for creating singleton MonoBehaviour classes.
/// </summary>
/// <typeparam name="T">The type of the singleton class.</typeparam>
public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
    //  ------------------ Public ------------------
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static T Instance { get; private set; }

    /// <summary>
    /// Performs additional initialization after the singleton instance is created.
    /// </summary>
    public abstract void PostAwake();

    //  ------------------ Private ------------------
    /// <summary>
    /// Unity's Awake method for singleton initialization.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = (T)this;
        DontDestroyOnLoad(gameObject);
        PostAwake();
    }
}
