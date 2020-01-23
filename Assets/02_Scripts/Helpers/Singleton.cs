using UnityEngine;

/// <summary>
/// Create singleton from this class. Himself destroy this instance if already have any instances.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    protected virtual void Awake()
    {
        if (instance.IsNullOrDefault())
            instance = (T)FindObjectOfType(typeof(T));
        else if (instance == this)
            return;
        else
            DestroyDuplicate();
    }

    protected virtual void DestroyDuplicate()
    {
        Destroy(this);
    }

    public static T Instance
    {
        get
        {
#if LOG_DETAILED || LOG_DETAILED_MAX
                if (instance == null)
                    Debug.LogError("[Singleton] Try to get access to " + typeof(T) + ", but can't find it.");
#endif
            return instance;
        }
    }
}