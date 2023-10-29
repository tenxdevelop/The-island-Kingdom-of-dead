using System;
using UnityEditor;
using UnityEngine;

public class UpdatableTerrainData : ScriptableObject
{
    public event Action OnValuesUpdatedEvent;

    [SerializeField] private bool m_autoUpdate;

    public bool autoUpdate => m_autoUpdate;
#if UNITY_EDITOR
    public void NotifyOfUpdatedValues()
    {
        EditorApplication.update -= NotifyOfUpdatedValues;
        OnValuesUpdatedEvent?.Invoke();
    }

    protected virtual void OnValidate()
    {
        if (autoUpdate)
        {
            EditorApplication.update += NotifyOfUpdatedValues;
        }
    }
#endif
}
