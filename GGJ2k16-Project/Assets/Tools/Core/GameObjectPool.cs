using UnityEngine;
using System.Collections;

/// <summary>
/// A Pool<T> wrapper for GameObjects.
/// If you understand how to set up the Pool<T>
/// Properly it'd be a better idea to use that so you
/// can customise enable / dissable. 
/// </summary>
public class GameObjectPool : Pool<GameObject>
{
    public GameObject m_prefab;
    
    public GameObjectPool(GameObject prefab = null) : base()
    {
        m_prefab = prefab;
        f_initalise = init;
        f_enabled = enable;
        f_dissabled = dissable;
    }

    private object init()
    {
        return GameObject.Instantiate(m_prefab);
    }

    private static void enable(GameObject obj)
    {
        obj.SetActive(true);
    }
    private static void dissable(GameObject obj)
    {
        obj.SetActive(false);
    }
}
