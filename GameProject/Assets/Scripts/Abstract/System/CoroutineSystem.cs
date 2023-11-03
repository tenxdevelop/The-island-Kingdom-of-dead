using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CoroutineSystem : MonoBehaviour
{
    private static CoroutineSystem m_instance;
    private static CoroutineSystem instance
    {
        get
        {
            if (m_instance == null)
            {
                var go = new GameObject("[CoroutineSystem]");
                m_instance = go.AddComponent<CoroutineSystem>();
                DontDestroyOnLoad(go);
            }
            return m_instance;
        }
        
    }
    public static Coroutine StartRoutine(IEnumerator routine)
    {
        return instance.StartCoroutine(routine);
    }

    public static void StopRoutine(Coroutine routine)
    {
        if (routine != null)
        {
            instance.StopCoroutine(routine);
        }
    }
}
