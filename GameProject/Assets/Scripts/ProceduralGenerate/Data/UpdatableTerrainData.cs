using System;
using UnityEngine;

public class UpdatableTerrainData : ScriptableObject
{
    public event Action OnValuesUpdatedEvent;

    [SerializeField] private bool m_autoUpdate;

    public bool autoUpdate => m_autoUpdate;

    public void NotifyOfUpdatedValues()
    {
        OnValuesUpdatedEvent?.Invoke();
    }

    protected virtual void OnValidate()
    {
        if (autoUpdate)
        {
            NotifyOfUpdatedValues();
        }
    }
}
