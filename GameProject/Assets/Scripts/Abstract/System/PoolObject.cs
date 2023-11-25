using System.Collections.Generic;
using UnityEngine;

public abstract class PoolObject<T> where T : MonoBehaviour
{
    public T prefab { get; }
    public Transform parent { get; }
    public bool autoExtend { get; set; }

    private List<T> m_pool;
    public PoolObject(T prefab, int count, Transform parent) : this(prefab, count, parent, false) { }
    public PoolObject(T prefab, int count, Transform parent, bool autoExtend = false)
    {
        this.prefab = prefab;
        this.autoExtend = autoExtend;
        this.parent = parent;
        CreatePool(count);
    }

    public T GetFreeObject()
    {
        if(HasFreeObject(out var prefab))
            return prefab;
        if (autoExtend)
            return CreateObject();
        Debug.Log($"Don't have free pool object type: {typeof(T)}");
        return null;
    }


    public bool HasFreeObject(out T prefab)
    {
        prefab = m_pool.Find(i => i.gameObject.activeInHierarchy);
        return prefab != null;
    }

    private void CreatePool(int count)
    {
        m_pool = new List<T>();
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isVisible = false)
    {
        var currentPrefab = Object.Instantiate(prefab, parent);
        currentPrefab.gameObject.SetActive(isVisible);
        m_pool.Add(currentPrefab);
        return currentPrefab;
    }
}
