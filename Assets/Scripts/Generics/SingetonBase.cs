using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
    public static T Instance { get; private set; }
    public abstract void PostAwake(); 
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