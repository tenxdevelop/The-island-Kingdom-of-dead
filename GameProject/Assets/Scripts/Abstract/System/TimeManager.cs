using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public const int HOUR_IN_DAY = 24;

    public static TimeManager instance;

    [SerializeField] private int m_dayDurationInSeconds = 24;

    private int m_currentHour = 0;
    private int m_dayInGame = 1;
    private float m_currentTimeOfDay = 0.5f;
    private bool m_lockNextDayTrigger = false;

    public int dayDurationInSeconds => m_dayDurationInSeconds;
    public float currentHour => m_currentHour;
    public float currentTimeOfDay => m_currentTimeOfDay;
    public int dayInGame => m_dayInGame;
    public int timeBlendedSkyBox => m_dayDurationInSeconds / HOUR_IN_DAY;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }

    private void Update()
    {
        m_currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        m_currentTimeOfDay %= 1;
        UpdateHour(Mathf.FloorToInt(m_currentTimeOfDay * HOUR_IN_DAY));
        TriggerNextDay();
    }

    private void TriggerNextDay()
    {
        if (currentHour == 0 && !m_lockNextDayTrigger)
        {
            m_dayInGame++;
            Debug.Log("day: " + dayInGame);
            m_lockNextDayTrigger = true;
        }
        if (currentHour != 0)
        {
            m_lockNextDayTrigger = false;
        }
        
    }

    private void UpdateHour(int hour)
    {
        m_currentHour = hour;
    }
}
