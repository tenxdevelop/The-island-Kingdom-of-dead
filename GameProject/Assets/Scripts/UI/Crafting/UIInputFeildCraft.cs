using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInputFeildCraft : MonoBehaviour
{
    public static Action OnChangeCountCraftEvent;
    public static UIInputFeildCraft instance { get; private set; }

    private InputField m_inputField;

    private int m_countCraft = 1;
    public int countCraft => m_countCraft;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }

    private void Start()
    {
        m_inputField = GetComponent<InputField>();
    }

    public void AddCount()
    {
        m_countCraft += 1;
        UpdateCount();
        OnChangeCountCraftEvent?.Invoke();
    }

    public void FieldCount()
    {
        m_countCraft = int.Parse(m_inputField.text);
        OnChangeCountCraftEvent?.Invoke();
    }

    public void ReduceCount()
    {
        m_countCraft -= 1;
        if (m_countCraft <= 0)
        {
            m_countCraft = 1;
        }
        UpdateCount();
        OnChangeCountCraftEvent?.Invoke();
    }
    private void UpdateCount()
    {
        m_inputField.text = m_countCraft.ToString();
    }
}
